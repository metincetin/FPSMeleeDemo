using System;
using System.Collections;
using System.Collections.Generic;
using FPSMeleeDemo.Core;
using FPSMeleeDemo.Gameplay;
using FPSMeleeDemo.Gameplay.BattleCharacters;
using UnityEngine;
using UnityEngine.UIElements;

namespace FPSMeleeDemo.UI.Tests
{
	public class CursorTest : MonoBehaviour
	{
		private UIDocument _document;

		private void Awake()
		{
			_document = GetComponent<UIDocument>();

			_document.rootVisualElement.Q<Button>("InvokeHitButton").clicked += OnCursorDamageButtonClicked;
			_document.rootVisualElement.Q<Button>("InvokeDamageTextButton").clicked += OnDamageTextButtonClicked;
		}

		private void OnDamageTextButtonClicked()
		{
			var damageObject = DamageBuilder.Create(UnityEngine.Random.Range(1, 35)).SetDamagePosition(UnityEngine.Random.onUnitSphere * 5).Build();
			EventBus<DamageTextInvoker.DamageTextEvent>.Invoke(new DamageTextInvoker.DamageTextEvent
			{
				Object = damageObject
			});
		}

		private void OnCursorDamageButtonClicked()
		{
			EventBus<CursorDamageEvent>.Invoke(new CursorDamageEvent());
		}

		public class CursorDamageEvent : IEvent
		{

		}
	}
}
