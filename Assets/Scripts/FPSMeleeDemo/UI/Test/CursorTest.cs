using System;
using System.Collections;
using System.Collections.Generic;
using FPSMeleeDemo.Core;
using UnityEngine;
using UnityEngine.UIElements;

namespace FPSMeleeDemo.UI.Tests
{
	public class CursorTest : MonoBehaviour
	{
        private UIDocument _document;

        private Button _invokeDamageButton;

        private void Awake()
		{
			_document = GetComponent<UIDocument>();

			_invokeDamageButton = _document.rootVisualElement.Q<Button>("InvokeHitButton");
		}

		private void OnEnable()
		{
			_invokeDamageButton.clicked += OnButtonClicked;
		}

		private void OnDisable()
		{
			_invokeDamageButton.clicked -= OnButtonClicked;
		}

        private void OnButtonClicked()
        {
			EventBus<CursorDamageEvent>.Invoke(new CursorDamageEvent());
        }

		public class CursorDamageEvent: IEvent
		{

		}
    }
}
