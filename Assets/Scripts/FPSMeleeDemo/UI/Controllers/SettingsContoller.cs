using FPSMeleeDemo.Input;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace FPSMeleeDemo.UI.Tests
{
	public class SettingContainer : MonoBehaviour
	{
		[SerializeField]
		private UIDocument _document;

		[SerializeField]
		private InputFeeder _inputFeeder;

        private Button _settingsButton;
        private VisualElement _settingsPanel;

        private void Awake()
		{
			_settingsButton = _document.rootVisualElement.Q<Button>("SettingsButton");
			_settingsPanel = _document.rootVisualElement.Q("Settings");
		}

		private void OnEnable()
		{
			_settingsButton.clicked += OnSettingsButtonClicked;
			_inputFeeder.GameInput.UI.ToggleSettings.performed += OnToggleSettingsPerformed;
		}

		private void OnDisable()
		{
			_settingsButton.clicked += OnSettingsButtonClicked;
			_inputFeeder.GameInput.UI.ToggleSettings.performed -= OnToggleSettingsPerformed;
		}

        private void OnToggleSettingsPerformed(InputAction.CallbackContext context)
        {
			ToggleSettings();
        }

        private void OnSettingsButtonClicked()
        {
			ToggleSettings();
			_settingsButton.Blur();
        }

		private void ToggleSettings()
		{
			_settingsPanel.ToggleInClassList("hidden");

			_inputFeeder.SetCursorState(!_settingsPanel.ClassListContains("hidden"));
		}
    }
}
