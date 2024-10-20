using FPSMeleeDemo.Gameplay;
using UnityEngine;

namespace FPSMeleeDemo.Rigging
{
	public class BodyPart : MonoBehaviour
	{
		public enum BodyPartType
		{
			Head,
			Chest,
			Leg,
			Arm
		}

		[SerializeField]
		private float _damageMultiplier = 1f;
		public float DamageMultiplier => _damageMultiplier;

		[SerializeField]
		private float _criticalChanceMultiplier = 1f;
		public float CriticalChanceMultiplier => _criticalChanceMultiplier;

		[SerializeField]
		private BodyPartType _bodyPartType;

		public IDamageReceiver BoundDamageReceiver { get; private set; }

		private void Awake()
		{
			BoundDamageReceiver = GetComponentInParent<IDamageReceiver>();
		}

	}
}
