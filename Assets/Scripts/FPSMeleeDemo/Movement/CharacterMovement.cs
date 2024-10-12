using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPSMeleeDemo.Movement
{

    public class CharacterMovement : MonoBehaviour, IMovement
	{

		public bool IsGrounded => _characterController.isGrounded;

		[SerializeField]
		private bool _alignToOrientation;

		private Vector2 _movementInput;
		public Vector2 MovementInput
		{
			get => _movementInput;
			set
			{
				_movementInput = Vector2.ClampMagnitude(value, 1f);
			}
		}

		public List<IMovementVelocityManipulator> Manipulators = new();

		public Vector3 Velocity => _characterController.velocity;

		[SerializeField]
		private CharacterController _characterController;

		private float _verticalVelocity;

		[SerializeField]
		private float _movementSpeed;

		[SerializeField]
		private float _jumpPower;

		private bool _jumpRequested;

		private void Update()
		{
			Move();
		}

		private void Move()
		{
			Vector3 velocity = Vector3.zero;

			if (IsGrounded)
			{
				_verticalVelocity = Physics.gravity.y * Time.deltaTime;
			}
			else
			{
				_verticalVelocity += Physics.gravity.y * Time.deltaTime;
			}

			if (_jumpRequested)
			{
				_jumpRequested = false;
				_verticalVelocity = _jumpPower;
			}


			var transformedInputVector = new Vector3(_movementInput.x, 0, _movementInput.y);

			if (_alignToOrientation)
			{
				transformedInputVector = Quaternion.Euler(0, transform.eulerAngles.y, 0) * transformedInputVector;
			}

			var inputVelocity = _movementSpeed * transformedInputVector;

			velocity = (inputVelocity + (Vector3.up * _verticalVelocity) + GetTotalManipulatorVelocity()) * Time.deltaTime;
			Debug.Log(inputVelocity);

			_characterController.Move(velocity);
		}

        private Vector3 GetTotalManipulatorVelocity()
        {
			Vector3 result = Vector3.zero;
			foreach(var m in Manipulators)
			{
				result += m.GetVelocity();
			}
			return result;
        }

        public void Jump()
		{
			if (!IsGrounded && !_jumpRequested) return;
			_jumpRequested = true;
		}
	}
}
