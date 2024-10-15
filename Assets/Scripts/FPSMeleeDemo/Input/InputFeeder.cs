using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FPSMeleeDemo.Input
{
	[CreateAssetMenu(menuName = "FPS/Input Feeder")]
	public class InputFeeder : ScriptableObject
	{
		private GameInput _gameInput;
		public GameInput GameInput 
		{
			get 
			{
				if (_gameInput == null)
				{
					_gameInput = new GameInput();
				}
				return _gameInput;
			}
		}

		private bool _isCursorTemporallyVisible;
        private bool _cursorState;

        public void Enable()
		{
			GameInput.Enable();

			GameInput.UI.ShowCursor.performed += OnShowCursorPerformed;
			GameInput.UI.ShowCursor.canceled += OnShowCursorCanceled;
		}
		
		public void Disable()
		{
			GameInput.Disable();

			GameInput.UI.ShowCursor.performed -= OnShowCursorPerformed;
			GameInput.UI.ShowCursor.canceled -= OnShowCursorCanceled;
		}

        private void OnShowCursorCanceled(InputAction.CallbackContext context)
        {
			_isCursorTemporallyVisible = false;
			GameInput.Player.Enable();
			UpdateCursor();
        }

        private void OnShowCursorPerformed(InputAction.CallbackContext context)
        {
			_isCursorTemporallyVisible = true;
			GameInput.Player.Disable();
			UpdateCursor();
        }

        public void SetCursorState(bool value)
		{
			_cursorState = value;
			if (value)
				GameInput.Player.Disable();
			else
				GameInput.Player.Enable();

			UpdateCursor();
		}

		private void UpdateCursor()
		{
			bool value = _cursorState || _isCursorTemporallyVisible;

			Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Locked;
			Cursor.visible = value;
		}
	}
}
