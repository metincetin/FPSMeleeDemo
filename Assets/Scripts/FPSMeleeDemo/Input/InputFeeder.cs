using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPSMeleeDemo.Input
{
	[CreateAssetMenu(menuName = "FPS/Input Feeder")]
	public class InputFeeder : ScriptableObject
	{
		private GameInput _gameInput;
		public GameInput GameInput => _gameInput;

		public void Enable()
		{
			if (_gameInput == null)
				_gameInput = new GameInput();
			_gameInput.Enable();
		}
		
		public void Disable()
		{
			_gameInput.Disable();
		}

		public void SetCursor(bool value)
		{
			Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Locked;
			Cursor.visible = value;
		}
	}
}
