using System;
using System.Collections;
using System.Collections.Generic;
using FPSMeleeDemo.BattleSystem;
using FPSMeleeDemo.Data;
using FPSMeleeDemo.FPS;
using FPSMeleeDemo.Montage;
using FPSMeleeDemo.Movement;
using FPSMeleeDemo.Rigging;
using UnityEngine;

namespace FPSMeleeDemo.Gameplay.BattleCharacters
{
	public class ImpactAnimator
	{
        private readonly Animator _animator;

        public ImpactAnimator(Animator animator)
		{
			_animator = animator;
		}

		public void PlayHit(Vector3 hitVector)
		{
			hitVector = _animator.transform.TransformDirection(hitVector);
			_animator.SetTrigger("Hit");
			var horizontal = 0f;
			if (Mathf.Abs(hitVector.x) > Mathf.Abs(hitVector.z))
			{
				horizontal = hitVector.x;
			}
			else
			{
				horizontal = hitVector.z;
			}

			_animator.SetFloat("HitDirectionX", horizontal);
			_animator.SetFloat("HitDirectionY", hitVector.y);
		}
	}

	public class AIBattleCharacter : MonoBehaviour, IBattleCharacter, IBattleObject
	{
		public Attributes Attributes { get; private set; }
		public BattleInstance BattleInstance { get; set; }
		
		private CharacterLocomotionAnimator _locomotionAnimator;
		private ImpactAnimator _impactAnimator;

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
	        Destroy(gameObject);
        }
    }
}
