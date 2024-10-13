using FPSMeleeDemo.Data;
using UnityEngine;

namespace FPSMeleeDemo.FPS
{
	public interface IWeaponInstanceContainer
	{
		public WeaponInstance WeaponInstance { get; }
	}

	public interface IAttacker
	{
		public void Attack(Vector2 direction);
	}
}

