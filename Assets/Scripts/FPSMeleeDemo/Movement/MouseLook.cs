using System.Collections;
using System.Collections.Generic;
using FPSMeleeDemo.Input;
using UnityEngine;

namespace FPSMeleeDemo.Movement
{
	public class MouseLook : MonoBehaviour
	{

		[SerializeField]
		private float _maxRotation;

		private float _verticalAngle;

		[SerializeField]
		private InputFeeder _inputFeeder;

		[SerializeField]
		private float _lookSensitivity;


		[SerializeField]
		private Transform _horizontalRotationTarget;

		[SerializeField]
		private Transform _verticalRotationTarget;

		private void Update()
		{
			var delta = _inputFeeder.GameInput.Player.Look.ReadValue<Vector2>();

			_horizontalRotationTarget.eulerAngles += Vector3.up * delta.x * _lookSensitivity;

			_verticalAngle -= delta.y * _lookSensitivity;
			_verticalAngle = Mathf.Clamp(_verticalAngle, -_maxRotation, _maxRotation);

			_verticalRotationTarget.localEulerAngles = new Vector3(_verticalAngle, 0, 0);
		}
	}
}
