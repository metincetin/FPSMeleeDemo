using DG.Tweening;
using FPSMeleeDemo.Core;
using FPSMeleeDemo.Gameplay.BattleCharacters;
using UnityEngine;
using UnityEngine.UI;

namespace FPSMeleeDemo.UI.Controllers
{
	public class WinViewController : MonoBehaviour
	{
		private CanvasGroup _canvasGroup;
		
		private Canvas _canvas;

		private EventBus<AIBattleCharacter.AIDeathEvent>.EventHandle _playerDiedEventHandle;
		
		private void Awake()
		{
			_canvas = GetComponent<Canvas>();
			_canvasGroup = GetComponent<CanvasGroup>();
		}

		private void OnEnable()
		{
			_playerDiedEventHandle = EventBus<AIBattleCharacter.AIDeathEvent>.Register(OnEnemyDied);
		}
		private void OnDisable()
		{
			EventBus<AIBattleCharacter.AIDeathEvent>.Unregister(_playerDiedEventHandle);
			this.DOKill();
		}

		private void OnEnemyDied(AIBattleCharacter.AIDeathEvent obj)
		{
			_canvas.enabled = true;
			_canvasGroup.DOFade(1, 1f).From(0).SetTarget(this);
		}
	}
}