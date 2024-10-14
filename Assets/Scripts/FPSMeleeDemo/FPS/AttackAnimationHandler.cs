using System;
using System.Collections;
using FPSMeleeDemo.Montage;
using UnityEngine;
using UnityEngine.Playables;

namespace FPSMeleeDemo.FPS
{

    public class AttackAnimationHandler
	{
		private MontagePlayer _montagePlayer;
		private Animator _animator;
        private IKHandler _ikHandler;
        private Coroutine _bounceCoroutine;

        public AttackAnimationHandler(MontagePlayer montagePlayer)
		{
			this._montagePlayer = montagePlayer;
			this._animator = montagePlayer.GetComponentInChildren<Animator>();
			this._ikHandler = _animator.gameObject.AddComponent<IKHandler>();
		}

		public void Play(PlayableAsset asset)
		{
			if (_bounceCoroutine != null)
			{
				_montagePlayer.StopCoroutine(_bounceCoroutine);
				ResetBounce();
			}

			_montagePlayer.PlayMontage(asset);
		}

		public void OnAttackHitReceived(DamageArea.DamageHitInfo info)
		{
			_bounceCoroutine = _montagePlayer.StartCoroutine(BounceCoroutine(info));
		}

		private IEnumerator BounceCoroutine(DamageArea.DamageHitInfo info)
		{
			float t = 0;

			_montagePlayer.PlayableDirector.timeUpdateMode = DirectorUpdateMode.Manual;

			while (_montagePlayer.PlayableDirector.time > 0)
			{

				_montagePlayer.PlayableDirector.time -= Time.deltaTime * 0.2;
				_montagePlayer.PlayableDirector.DeferredEvaluate();

				t += Time.deltaTime;

				yield return new WaitForEndOfFrame();
			}
			ResetBounce();
		}

		private void ResetBounce()
		{
			_montagePlayer.PlayableDirector.timeUpdateMode = DirectorUpdateMode.GameTime;
			_montagePlayer.PlayableDirector.time = 0;
		}
	}
}

