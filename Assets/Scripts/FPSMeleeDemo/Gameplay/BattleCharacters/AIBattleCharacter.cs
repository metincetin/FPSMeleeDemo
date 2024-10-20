using System;
using System.Collections;
using System.Collections.Generic;
using FPSMeleeDemo.BattleSystem;
using FPSMeleeDemo.FPS;
using FPSMeleeDemo.Montage;
using FPSMeleeDemo.Movement;
using FPSMeleeDemo.Rigging;
using UnityEngine;

namespace FPSMeleeDemo.Gameplay.BattleCharacters
{
	public class AIBattleCharacter : MonoBehaviour, IBattleCharacter, IBattleObject
	{
		public Attributes Attributes { get; private set; }
		public BattleInstance BattleInstance { get; set; }
		
		private CharacterLocomotionAnimator _locomotionAnimator;

		private IMovement _movement;

		private IAttacker _attacker;

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
			var target = BattleInstance.GetOther(this);

			if (target is not MonoBehaviour mb) return;

			var dir = mb.transform.position - transform.position;
			dir.y = 0;
			dir.Normalize();

			var rot = Quaternion.LookRotation(dir);

			transform.rotation = rot;

			_locomotionAnimator.MovementInput = _movement.MovementInput;
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
			if (attr.Health <= 0)
			{
				Die();
			}

			DamageReceived?.Invoke(damage);
        }

        private void Die()
        {
        }
    }
}
