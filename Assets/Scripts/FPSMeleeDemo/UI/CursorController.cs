using System.Collections;
using FPSMeleeDemo.Core;
using UnityEngine;
using UnityEngine.UIElements;
using static FPSMeleeDemo.UI.Tests.CursorTest;

namespace FPSMeleeDemo.UI
{
    public class CursorController: MonoBehaviour
	{
		[SerializeField]
		private UIDocument _document;

        private EventBus<CursorDamageEvent>.EventHandle _handle;

        private Cursor _view;
        private Coroutine _damageCoroutine;

        private void OnEnable()
		{
			_handle = EventBus<CursorDamageEvent>.Register(OnCursorDamage);
			_view = _document.rootVisualElement.Q<Cursor>("Cursor");
		}


        private void OnDisable()
		{
			EventBus<CursorDamageEvent>.Unregister(_handle);
		}

        private void OnCursorDamage(CursorDamageEvent @event)
        {
			_damageCoroutine = StartCoroutine(ShowDamage());
        }

		private IEnumerator ShowDamage()
		{
			_view.ShowDamage();
			yield return new WaitForSeconds(.4f);
			_view.HideDamage();
		}
	}
}

