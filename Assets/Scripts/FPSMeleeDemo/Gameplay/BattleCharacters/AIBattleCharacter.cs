using System;
using System.Collections;
using System.Collections.Generic;
using FPSMeleeDemo.AI;
using FPSMeleeDemo.BattleSystem;
using FPSMeleeDemo.Core;
using FPSMeleeDemo.Data;
using FPSMeleeDemo.FPS;
using FPSMeleeDemo.Montage;
using FPSMeleeDemo.Movement;
using FPSMeleeDemo.Rigging;
using UnityEngine;

namespace FPSMeleeDemo.Gameplay.BattleCharacters
{
	public class AIBattleCharacter : MonoBehaviour, IBattleCharacter, IBattleObject
	{
		public struct AIDeathEvent : IEvent { }
		public Attributes Attributes { get; private set; }

		public BattleInstance BattleInstance { get; set; }
		
		private CharacterLocomotionAnimator _locomotionAnimator;
		private ImpactAnimator _impactAnimator;

		private IMovement _movement;

		private IAttacker _attacker;
		
		private bool _isDead;

		public event Action<DamageObject> DamageReceived;

        private void Awake()
		{
			Attributes = new Attributes { Damage = 5, Defence = 2, Health = 50, MaxHealth = 50 };

			_movement = GetComponent<IMovement>();

			if (_movement is CharacterMovement characterMovement)
			{
				characterMovement.Manipulators.Add(new RootMotionVelocityManipulator(GetComponentInChildren<RootMotionController>()));
			}

			Animator animator = GetComponentInChildren<Animator>();
			_locomotionAnimator = new CharacterLocomotionAnimator(animator);

			_impactAnimator = new ImpactAnimator(animator);

			_attacker = GetComponent<IAttacker>();

			_attacker.AnimationHandler = new AIAttackAnimationHandler(GetComponent<MontagePlayer>());
		}
		
		private void Start()
		{
			var ikLook = GetComponentInChildren<IKTargetLook>();
			if (ikLook)
				ikLook.Target = ((MonoBehaviour)BattleInstance.GetOther(this)).transform;
		}

		public void Update()
		{
			if (_isDead) return;
			var target = BattleInstance.GetOther(this);

			if (target is not MonoBehaviour mb) return;

			var dir = mb.transform.position - transform.position;
			dir.y = 0;
			dir.Normalize();

			var rot = Quaternion.LookRotation(dir);

			transform.rotation = rot;

			_locomotionAnimator.MovementInput = new Vector2(_movement.Velocity.x, _movement.Velocity.z);
			_locomotionAnimator.Update();
		}

        public void ApplyDamage(DamageObject damage)
        {
			var blockHandler = _attacker.BlockHandler;

			if (blockHandler.IsBlocking)
			{
				blockHandler.DepleteDamage(damage);
			}

			if (damage.Damage <= 0) return;
			
			var attr = Attributes;
			attr.Health -= damage.Damage;
			Attributes = attr;

			_impactAnimator.PlayHit(damage.DamageVelocity.normalized);

			if (attr.Health <= 0)
			{
				Die();
			}

			DamageReceived?.Invoke(damage);
        }

        private void Die()
        {
	        if (_isDead) return;
	        _isDead = true;
	        EventBus<AIDeathEvent>.Invoke(new AIDeathEvent());
	        var ragdollSwitcher = GetComponentInChildren<RagdollSwitcher>();
	        if (ragdollSwitcher)
			{
				ragdollSwitcher.SetRagdoll();
			}
	        if (TryGetComponent<CharacterController>(out var characterController))
	        {
		        characterController.enabled = false;
	        }
        }
        
        public void OnBattleEnded()
        {
	        if (TryGetComponent<AIController>(out var aiController))
	        {
		        aiController.End();
	        }
        }
    }
}
