using System;
using FPSMeleeDemo.Input;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace FPSMeleeDemo.UI.Tests
{
    public class SettingsController : MonoBehaviour
	{
		[SerializeField]
		private InputFeeder _inputFeeder;

		private Settings _settings;

		private void Awake()
		{
			_settings = GetComponent<Settings>();
		}

		private void OnEnable()
		{
			_inputFeeder.GameInput.UI.ToggleSettings.performed += OnToggleSettingsPerformed;
		}

		private void OnDisable()
		{
			_inputFeeder.GameInput.UI.ToggleSettings.performed -= OnToggleSettingsPerformed;
		}

		private void OnToggleSettingsPerformed(InputAction.CallbackContext context)
		{
			ToggleSettings();
		}

		private void ToggleSettings()
		{
			var value = !_settings.IsOpen;

			if (value) _settings.Open();
			else
				_settings.Close();

			_inputFeeder.SetCursorState(value);
		}
	}
}
