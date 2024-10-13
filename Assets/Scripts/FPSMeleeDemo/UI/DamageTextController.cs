using System.Collections;
using FPSMeleeDemo.Core;
using FPSMeleeDemo.Gameplay.BattleCharacters;
using UnityEngine;
using UnityEngine.UIElements;

namespace FPSMeleeDemo.UI
{
    public class DamageTextController : MonoBehaviour
	{
		[SerializeField]
        private UIDocument _document;

		private Camera _camera;

        private EventBus<DamageTextInvoker.DamageTextEvent>.EventHandle _handle;
        private VisualElement _view;

		private void Awake()
		{
			_camera = Camera.main;
		}

        private void OnEnable()
		{
			_handle = EventBus<DamageTextInvoker.DamageTextEvent>.Register(OnDamageTextRequsted);
			_view = _document.rootVisualElement.Q("DamageTextContainer");
		}

        private void OnDisable()
		{
			EventBus<DamageTextInvoker.DamageTextEvent>.Unregister(_handle);
		}


        private void OnDamageTextRequsted(DamageTextInvoker.DamageTextEvent ev)
        {
			var inst = new Label();

			inst.text = ev.Object.Damage.ToString();
			inst.AddToClassList("damage-text");


			_view.Add(inst);

			StartCoroutine(UpdateLabelPosition(inst, ev.Object.DamagePosition));
			StartCoroutine(HandleLabelLifetime(inst));
        }

		private IEnumerator UpdateLabelPosition(Label inst, Vector3 damagePosition)
		{
			float time = Time.time;
			while(time < Time.time + 1)
			{
				var pos = _camera.WorldToScreenPoint(damagePosition);

				inst.style.top = pos.y;
				inst.style.left = pos.x;
				yield return new WaitForEndOfFrame();
			}
		}

        private IEnumerator HandleLabelLifetime(Label inst)
        {
			yield return new WaitForEndOfFrame();

			inst.style.translate = new StyleTranslate(new Translate(0, -50));

			yield return new WaitForSeconds(0.2f);

			inst.style.opacity = 0;

			yield return new WaitForSeconds(2f);
			inst.RemoveFromHierarchy();

        }
    }
}


