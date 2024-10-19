using FPSMeleeDemo.Gameplay;
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

		private void Start()
		{
			BattleSettings settings = new();

			IBattleCharacter[] characters = new IBattleCharacter[2];

			characters[0] = Instantiate(_playerPrefab, Vector3.zero, Quaternion.identity).GetComponentInChildren<IBattleCharacter>();
			characters[1] = Instantiate(_starterEnemyPrefab, new Vector3(0,0,20), Quaternion.identity).GetComponentInChildren<IBattleCharacter>();

			settings.Characters = characters;

			_battleController.CreateBattle(settings);
		}
	}
}

