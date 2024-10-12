using System;
using System.Collections;
using System.Collections.Generic;
using FPSMeleeDemo.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FPSMeleeDemo.Movement.Tests
{
	public class MovementTest : MonoBehaviour
	{
		private CharacterMovement _movement;

		[SerializeField]
		private InputFeeder _inputFeeder;

		private void Awake()
		{
			_movement = GetComponent<CharacterMovement>();
			_movement.Manipulators.Add(new RootMotionVelocityManipulator(GetComponentInChildren<RootMotionController>()));
		}

		private void OnEnable()
		{
			_inputFeeder.Enable();

			_inputFeeder.SetCursor(false);

			_inputFeeder.GameInput.Player.Jump.performed += OnJumpPressed;
		}

		private void OnDisable()
		{
			_inputFeeder.Disable();
			_inputFeeder.GameInput.Player.Jump.performed -= OnJumpPressed;
		}

        private void OnJumpPressed(InputAction.CallbackContext context)
        {
			_movement.Jump();
        }

        private void Update()
		{
			_movement.MovementInput = _inputFeeder.GameInput.Player.Movement.ReadValue<Vector2>();
		}
	}
}
