using UnityEngine;

namespace FPSMeleeDemo.Gameplay
{
    public struct DamageObject
	{
		public float Damage;
		public bool IsCritical;
		public Vector3 DamagePosition;
		public Vector3 DamageVelocity;

		public IDamageCauser Causer;
	}
}

