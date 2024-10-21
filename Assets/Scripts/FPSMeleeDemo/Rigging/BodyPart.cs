using System;
using DG.Tweening;
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

		[SerializeField] private float _damageMultiplier = 1f;
		public float DamageMultiplier => _damageMultiplier;

		[SerializeField] private float _criticalChanceMultiplier = 1f;
		public float CriticalChanceMultiplier => _criticalChanceMultiplier;

		[SerializeField] private BodyPartType _bodyPartType;

		public IDamageReceiver BoundDamageReceiver { get; private set; }

		private void Awake()
		{
			BoundDamageReceiver = GetComponentInParent<IDamageReceiver>();
		}

		private Tween _hitImpactTween;

		public void SimulateHitImpact(Vector3 force, Vector3 at)
		{
			SimulateHitImpactRecursive(this, force, at, 2);
		}
		
		private void SimulateHitImpactRecursive(BodyPart target, Vector3 force, Vector3 at, int depth)
		{
			if(target.TryGetComponent(out Rigidbody rigidbody))
			{
				rigidbody.isKinematic = false;
				rigidbody.AddForceAtPosition(force, at, ForceMode.Impulse);
			}
			
			if (depth > 0)
			{
				var ch = GetComponentInChildren<BodyPart>();
				if (ch)
					SimulateHitImpactRecursive(ch, force, at, depth - 1);

				var p = GetComponentInParent<BodyPart>();
				if (p)
					SimulateHitImpactRecursive(p, force, at, depth - 1);
			}
		}
	}
}