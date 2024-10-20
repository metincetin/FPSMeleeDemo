using System.Collections;
using FPSMeleeDemo.BattleSystem;
using FPSMeleeDemo.Gameplay.BattleCharacters;
using UnityEngine;

namespace FPSMeleeDemo.Gameplay
{
	public class PlayerInjector : MonoBehaviour
	{
		[SerializeField]
		private BattleController _battleController;

		private void Awake()
		{
			StartCoroutine(Inject());
		}

		private IEnumerator Inject()
		{
			yield return new WaitUntil(() => _battleController.CurrentBattle != null);

			var dependants = GetComponentsInChildren<IDependant<IBattleCharacter>>();
			var playerCharacter = _battleController.CurrentBattle.GetCharacterOfType<PlayerBattleCharacter>();

			foreach(var d in dependants)
			{
				d.Value = playerCharacter;
			}
		}
	}
}
