using System;
using System.Linq;
using UnityEngine;

namespace FPSMeleeDemo.Rigging
{

    public class AnimatedRigApplier : MonoBehaviour
	{
		[Serializable]
		public class PartPair
		{
			public Rigidbody Physical;
			public Transform Animated;
		}
		
		[SerializeField]
		private Transform _physicsRig;

		[SerializeField]
		private Transform _animatedRig;
		
		[SerializeField]
		private PartPair[] _partsMap;

		private void FixedUpdate()
		{
			for (int i = 0; i < _partsMap.Length; i++)
			{
				var part = _partsMap[i];

				if (part.Physical.isKinematic)
				{
					part.Physical.position = part.Animated.position;
					part.Physical.rotation = part.Animated.rotation;
				}
			}
		}
	}
}
