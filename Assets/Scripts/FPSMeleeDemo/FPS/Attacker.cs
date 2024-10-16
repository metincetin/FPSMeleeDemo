using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using FPSMeleeDemo.Data;
using FPSMeleeDemo.FX;
using FPSMeleeDemo.Gameplay;
using FPSMeleeDemo.Montage;
using UnityEngine;

namespace FPSMeleeDemo.FPS
{

    public class Attacker : MonoBehaviour, IAttacker, IWeaponInstanceContainer, IDamageCauser
	{
		[SerializeField]
		private Weapon _weapon;
		public Weapon Weapon
		{
			get => _weapon;
			set => _weapon = value;
		}

		public WeaponInstance WeaponInstance { get; private set; }

		private AttackAnimationHandler _attackAnimationHandler;

		private IAttackerFXFactory _fxFactory;

		private BlockHandler _blockHandler;

		[SerializeField]
		private Transform _weaponPoint;

		private IDamageCauser _damageCauser;

		public event Action<IDamageReceiver, DamageObject> DamageCaused;


		private void Awake()
		{
			_damageCauser = GetComponent<IDamageCauser>();
			_attackAnimationHandler = new AttackAnimationHandler(GetComponent<MontagePlayer>());
			_fxFactory = Resources.Load<AttackerFXFactory>("Factories/AttackerFXFactory");

			if (_weapon)
			{
				CreateWeapon();
			}

			_blockHandler = new BlockHandler();
		}

		private void CreateWeapon()
		{
			if (WeaponInstance) RemoveWeaponInstance();
			WeaponInstance = WeaponInstance.Create(_weapon, _weaponPoint);

			var damageArea = WeaponInstance.GetComponentInChildren<DamageArea>();
			if (damageArea)
			{
				damageArea.HitReceived += OnDamageAreaHitReceived;
				damageArea.AddIgnore(GetComponent<Collider>());
			}
		}

		private void RemoveWeaponInstance()
		{
			Destroy(WeaponInstance.gameObject);

			var damageArea = WeaponInstance.GetComponentInChildren<DamageArea>();
			if (damageArea)
			{
				damageArea.HitReceived -= OnDamageAreaHitReceived;
			}
		}

		private void OnDamageAreaHitReceived(DamageArea.DamageHitInfo info)
		{
			_attackAnimationHandler.OnAttackHitReceived(info);

			_fxFactory.CreateHitEffect(info);

			if (TryGetComponent<CinemachineImpulseSource>(out var impulseSource))
			{
				impulseSource.GenerateImpulseAtPositionWithVelocity(info.Point, Vector3.up * .050f);
			}

			var damageArea = WeaponInstance.GetComponentInChildren<DamageArea>();
			if (damageArea)
			{
				damageArea.End();
			}

			if (info.Collider.TryGetComponent<IDamageReceiver>(out var damageReceiver))
			{
				var damage = DamageBuilder.Create(5)
					.SetCauser(_damageCauser)
					.SetDamagePosition(info.Point)
					.Build();
				damageReceiver.ApplyDamage(damage);
				DamageCaused?.Invoke(damageReceiver, damage);
			}
		}

		public void Attack(Vector2 direction)
		{
			if (_attackAnimationHandler.IsAttacking) return;
			_attackAnimationHandler.Weapon = _weapon;
			_attackAnimationHandler.Play(direction.ToCardinal());
		}

		public void SetBlockState(bool value)
		{
			if (value)
			{
				_blockHandler.BeginBlock();
				_attackAnimationHandler.BeginBlock();
			}
			else
			{
				_blockHandler.EndBlock();
				_attackAnimationHandler.EndBlock();
			}

		}
	}
}
