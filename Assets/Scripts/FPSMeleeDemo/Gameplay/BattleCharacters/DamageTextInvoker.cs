using FPSMeleeDemo.Core;
using UnityEngine;

namespace FPSMeleeDemo.Gameplay.BattleCharacters
{

    public class DamageTextInvoker : MonoBehaviour
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
			EventBus<DamageTextEvent>.Invoke(new DamageTextEvent { Object = @object });
		}
	}
}

