using UnityEngine;

namespace FPSMeleeDemo.Gameplay
{
	public class DamageBuilder
	{
		private IDamageCauser _causer;
		private float _damage;
		private bool _isCritical;
		private Vector3 _damagePosition;

		public static DamageBuilder Create(float damage = 0)
		{
			return new DamageBuilder().SetBaseDamage(damage);
		}

		public DamageBuilder SetCauser(IDamageCauser value)
		{
			this._causer = value;
			return this;
		}

		public DamageBuilder SetBaseDamage(float value)
		{
			this._damage = value;
			return this;
		}
		public DamageBuilder MultiplyDamage(float value)
		{
			this._damage *= value;
			return this;
		}

		public DamageBuilder SetCritical(bool value)
		{
			this._isCritical = value;
			this._damage *= 2;
			return this;
		}

		public DamageBuilder SetCriticalByChance(float value)
		{
			this._isCritical = Random.value < value;
			return this;
		}
		public DamageBuilder SetDamagePosition(Vector3 value)
		{
			this._damagePosition = value;
			return this;
		}

		public DamageObject Build()
		{
			return new DamageObject
			{
				Damage = _damage,
				Causer = _causer,
				IsCritical = _isCritical,
				DamagePosition = _damagePosition
			};
		}
	}
}

