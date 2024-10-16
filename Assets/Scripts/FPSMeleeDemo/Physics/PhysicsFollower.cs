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

		public Transform Target
		{
			get => _target;
			set => _target = value;
		}

		private void Awake()
		{
			_rigidbody = GetComponent<Rigidbody>();
		}

		private void FixedUpdate()
		{
			if (!_target) return;
			_rigidbody.AddForce((_target.position - _rigidbody.position) * _force, ForceMode.VelocityChange);
			//_rigidbody.AddTorque((Quaternion.Inverse(_rigidbody.rotation) * _target.rotation).eulerAngles * _angularForce, ForceMode.VelocityChange);
			//
			
			var x = Vector3.Cross(transform.forward, _target.forward);
			float theta = Mathf.Asin(x.magnitude);
			var w = x.normalized * theta / Time.fixedDeltaTime;
			var q = transform.rotation * _rigidbody.inertiaTensorRotation;
			var t = q * Vector3.Scale(_rigidbody.inertiaTensor, Quaternion.Inverse(q) * w);
			_rigidbody.AddTorque(t - _rigidbody.angularVelocity, ForceMode.VelocityChange);

			// _rigidbody.rotation = _target.rotation;
		}
	}
}
