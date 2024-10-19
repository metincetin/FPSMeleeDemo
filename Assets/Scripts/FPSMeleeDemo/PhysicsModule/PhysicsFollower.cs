using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPSMeleeDemo.PhysicsModule
{
	public class PhysicsFollower : MonoBehaviour
	{
		[SerializeField]
		private Transform _target;

		private Rigidbody _rigidbody;

		[SerializeField]
		private float _force = 200;

		[SerializeField]
		private float _angularForce = 200;

		private AnimatedRigApplier _animatedRig;

		public Transform Target
		{
			get => _target;
			set => _target = value;
		}

		private void Awake()
		{
			_animatedRig = GetComponentInParent<AnimatedRigApplier>();
			_rigidbody = GetComponent<Rigidbody>();
		}

		private void FixedUpdate()
		{
			if (!_target) return;

			_animatedRig.GetAnimationTransform(transform, out var targetPosition, out var targetRotation);
			_rigidbody.AddForce((targetPosition - _rigidbody.position) * _force, ForceMode.VelocityChange);
			//_rigidbody.AddTorque((Quaternion.Inverse(_rigidbody.rotation) * _target.rotation).eulerAngles * _angularForce, ForceMode.VelocityChange);
			//
			//

			var targetForward = targetRotation * Vector3.forward;
			
			var x = Vector3.Cross(transform.forward, targetForward);
			float theta = Mathf.Asin(x.magnitude);
			var w = x.normalized * theta / Time.fixedDeltaTime;
			var q = transform.rotation * _rigidbody.inertiaTensorRotation;
			var t = q * Vector3.Scale(_rigidbody.inertiaTensor, Quaternion.Inverse(q) * w);
			_rigidbody.AddTorque(t - _rigidbody.angularVelocity, ForceMode.VelocityChange);

			// _rigidbody.rotation = _target.rotation;
		}
	}
}
