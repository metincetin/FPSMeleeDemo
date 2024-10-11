using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Spring = UnitySpring.ClosedForm.Spring;

namespace FPSMeleeDemo.FPS.Tests
{
	public class SpringTest : MonoBehaviour
	{
		private Spring _spring;

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
		private bool _autoUpdate;

		private float _v;


		private void Awake()
		{
			_prevPosition = transform.position.y;

			CreateSpring();
		}

		private void CreateSpring()
		{
			_spring = new Spring { startValue = _prevPosition, endValue = _target.position.y, damping = _damping, stiffness = _stiffness, mass = _mass };
		}

		private void OnValidate()
		{
			CreateSpring();
		}

		private void Update()
		{

			var t = _target.position.y;
			var pos = transform.position;

			float y = pos.y;

			SpringDamper.SpringDamperExactStiffnessDamping(ref y, ref _v, t, 0, _stiffness, _damping, Time.deltaTime);

			pos.y = y;


			transform.position = pos;
		}
	}
}
