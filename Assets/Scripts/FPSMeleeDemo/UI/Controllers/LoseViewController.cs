using System;
using DG.Tweening;
using FPSMeleeDemo.Core;
using FPSMeleeDemo.Gameplay.BattleCharacters;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace FPSMeleeDemo.UI.Controllers
{
	public class LoseViewController : MonoBehaviour
	{
		private CanvasGroup _canvasGroup;
		
		private Canvas _canvas;
		private GraphicRaycaster _raycaster;
		
		[SerializeField]
		private Button _restartButton;

		private EventBus<PlayerBattleCharacter.PlayerDeathEvent>.EventHandle _playerDiedEventHandle;
		
		private void Awake()
		{
			_canvas = GetComponent<Canvas>();
			_canvasGroup = GetComponent<CanvasGroup>();
			_raycaster = GetComponent<GraphicRaycaster>();
		}

		private void OnEnable()
		{
			_playerDiedEventHandle = EventBus<PlayerBattleCharacter.PlayerDeathEvent>.Register(OnPlayerDied);
			_restartButton.onClick.AddListener(OnRestartClicked);
		}
		private void OnDisable()
		{
			EventBus<PlayerBattleCharacter.PlayerDeathEvent>.Unregister(_playerDiedEventHandle);
			_restartButton.onClick.RemoveListener(OnRestartClicked);
			this.DOKill();
		}

		private void OnRestartClicked()
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
		

		private void OnPlayerDied(PlayerBattleCharacter.PlayerDeathEvent obj)
		{
			_canvas.enabled = true;
			_raycaster.enabled = true;
			_canvasGroup.DOFade(1, 1f).From(0).SetTarget(this);
		}
	}
}