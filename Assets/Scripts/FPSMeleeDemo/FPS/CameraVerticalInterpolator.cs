using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPSMeleeDemo.FPS
{
	public class CameraVerticalInterpolator : MonoBehaviour
	{

		private float _prevPosition;

		[SerializeField]
		private Transform _target;

		[SerializeField]
		private float _damping;

		[SerializeField]
		private float _stiffness;

		[SerializeField]
		private float _mass;

		[SerializeField]
		private float _offset;

        private float _velocity;

        private void Awake()
		{
			_prevPosition = transform.position.y;

		}

		private void LateUpdate()
		{

			var t = _target.position.y + _offset;
			var pos = transform.position;

			float y = _prevPosition;

			SpringDamper.SpringDamperExactStiffnessDamping(ref y, ref _velocity, t, 0, _stiffness, _damping, Time.deltaTime);


			pos.x = _target.position.x;
			pos.z = _target.position.z;

			pos.y = y;

			_prevPosition = y;

			transform.position = pos;
		}
	}
}
