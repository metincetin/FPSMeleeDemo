using FPSMeleeDemo.Core;
using UnityEngine;
using static FPSMeleeDemo.UI.Tests.CursorTest;

namespace FPSMeleeDemo.Gameplay.BattleCharacters
{
    public class DamageCursorInvoker : MonoBehaviour
	{
		private IDamageCauser _damageCauser;

		public struct DamageTextEvent : IEvent
		{
			public DamageObject Object;
		}

		private void Awake()
		{
			_damageCauser = GetComponent<IDamageCauser>();
		}

		private void OnEnable()
		{
			_damageCauser.DamageCaused += OnDamageCaused;
		}

		private void OnDisable()
		{
			_damageCauser.DamageCaused -= OnDamageCaused;
		}

		private void OnDamageCaused(IDamageReceiver receiver, DamageObject @object)
		{
			EventBus<CursorDamageEvent>.Invoke(new CursorDamageEvent());
		}
	}
}


