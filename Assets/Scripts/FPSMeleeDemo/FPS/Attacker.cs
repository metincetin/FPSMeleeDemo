using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using FPSMeleeDemo.Data;
using FPSMeleeDemo.FX;
using FPSMeleeDemo.Gameplay;
using FPSMeleeDemo.Montage;
using FPSMeleeDemo.Rigging;
using UnityEngine;

namespace FPSMeleeDemo.FPS
{
	public class Attacker : MonoBehaviour, IAttacker, IWeaponInstanceContainer, IDamageCauser, IAttackStateProvider
	{
		[SerializeField]
		private Weapon _weapon;
		public Weapon Weapon
		{
			get => _weapon;
			set => _weapon = value;
		}

		public WeaponInstance WeaponInstance { get; private set; }

		public AttackAnimationHandler AnimationHandler { get; set; }

		private IAttackerFXFactory _fxFactory;

		private BlockHandler _blockHandler;
		public BlockHandler BlockHandler => _blockHandler;

		bool IAttackStateProvider.IsAttacking => AnimationHandler.IsAttacking;

		[SerializeField]
		private Transform _weaponPoint;

		private IDamageCauser _damageCauser;

		public event Action<IDamageReceiver, DamageObject> DamageCaused;

		[SerializeField]
		private float _blockPowerPerSecond = 5;


		private void Awake()
		{
			_damageCauser = GetComponent<IDamageCauser>();
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
				var selfColliders = GetComponentsInChildren<Collider>();
				foreach(var c in selfColliders)
				{
					damageArea.AddIgnore(c);
				}
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
			AnimationHandler.OnAttackHitReceived(info);

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

			IDamageReceiver targetReceiver = null;

			var bodyPart = info.Collider.GetComponent<BodyPart>();


			var damage = DamageBuilder.Create(5)
				.SetCauser(_damageCauser)
				.SetDamagePosition(info.Point);

			if (bodyPart)
			{
				targetReceiver = bodyPart.BoundDamageReceiver;
				damage.MultiplyDamage(bodyPart.DamageMultiplier);
			}
			else
			{
				targetReceiver = info.Collider.GetComponent<IDamageReceiver>();
			}

			if (targetReceiver == null) return;

			var damageInstance = damage.Build();

			targetReceiver.ApplyDamage(damageInstance);

			DamageCaused?.Invoke(targetReceiver, damageInstance);
		}

		public void Attack(Vector2 direction)
		{
			if (AnimationHandler.IsAttacking) return;
			AnimationHandler.Weapon = _weapon;
			AnimationHandler.Play(direction.ToCardinal());

		}

		private void Update()
		{
			_blockHandler.AddBlockPower(_blockPowerPerSecond * Time.deltaTime);
		}

		public void SetBlockState(bool value)
		{
			if (value)
			{
				_blockHandler.BeginBlock();
				AnimationHandler.BeginBlock();
			}
			else
			{
				_blockHandler.EndBlock();
				AnimationHandler.EndBlock();
			}

		}
	}
}
