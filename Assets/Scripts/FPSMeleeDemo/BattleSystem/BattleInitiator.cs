using System;
using FPSMeleeDemo.Core;
using FPSMeleeDemo.Gameplay;
using FPSMeleeDemo.Gameplay.BattleCharacters;
using UnityEngine;

namespace FPSMeleeDemo.BattleSystem
{
    public class BattleInitiator : MonoBehaviour
	{
		[SerializeField]
		private GameObject _playerPrefab;

		[SerializeField]
		private BattleController _battleController;
		
		[SerializeField]
		public GameObject _starterEnemyPrefab;

		
		private EventBus<PlayerBattleCharacter.PlayerDeathEvent>.EventHandle _playerDiedEventHandle;
		private EventBus<AIBattleCharacter.AIDeathEvent>.EventHandle _aiDiedEventHandle;

		private void Start()
		{
			BattleSettings settings = new();

			IBattleCharacter[] characters = new IBattleCharacter[2];

			characters[0] = Instantiate(_playerPrefab, Vector3.zero, Quaternion.identity).GetComponentInChildren<IBattleCharacter>();
			characters[1] = Instantiate(_starterEnemyPrefab, new Vector3(0,0,20), Quaternion.identity).GetComponentInChildren<IBattleCharacter>();

			settings.Characters = characters;

			_battleController.CreateBattle(settings);
		}

		private void OnEnable()
		{
			_playerDiedEventHandle = EventBus<PlayerBattleCharacter.PlayerDeathEvent>.Register(OnPlayerDied);
			_aiDiedEventHandle = EventBus<AIBattleCharacter.AIDeathEvent>.Register(OnAIDied);
		}


		private void OnDisable()
		{
			EventBus<PlayerBattleCharacter.PlayerDeathEvent>.Unregister(_playerDiedEventHandle);
			EventBus<AIBattleCharacter.AIDeathEvent>.Unregister(_aiDiedEventHandle);
		}
		

		private void OnPlayerDied(PlayerBattleCharacter.PlayerDeathEvent obj)
		{
			_battleController.EndBattle();
		}
		
		private void OnAIDied(AIBattleCharacter.AIDeathEvent obj)
		{
			_battleController.EndBattle();
		}
	}
}

