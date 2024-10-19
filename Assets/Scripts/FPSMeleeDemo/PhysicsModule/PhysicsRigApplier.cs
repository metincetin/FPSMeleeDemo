using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPSMeleeDemo.PhysicsModule
{
	public class PhysicsRigApplier : MonoBehaviour
	{
		[SerializeField]
		private Transform _physicsRig;

		[SerializeField]
		private Transform _animatedRig;

		private void Awake()
		{
			var followers = _physicsRig.GetComponentsInChildren<PhysicsFollower>();

			var animatedParts = _animatedRig.GetComponentsInChildren<Transform>();

			foreach(var f in followers)
			{
				foreach(var animated in animatedParts)
				{
					if (animated.gameObject.name == f.gameObject.name)
						f.Target = animated;
				}
			}
		}
	}
}
