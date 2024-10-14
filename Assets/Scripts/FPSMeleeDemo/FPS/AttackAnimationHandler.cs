using System;
using System.Collections;
using FPSMeleeDemo.Data;
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

		public Weapon Weapon { get; set; }

		private CardinalDirection _attackDirection;

		private readonly int IsBlockingHash = Animator.StringToHash("IsBlocking");

		public AttackAnimationHandler(MontagePlayer montagePlayer)
		{
			this._montagePlayer = montagePlayer;
			this._animator = montagePlayer.GetComponentInChildren<Animator>();
			this._ikHandler = _animator.gameObject.AddComponent<IKHandler>();
		}

		public void Play(CardinalDirection direction)
		{
			_attackDirection = direction;
			_montagePlayer.PlayMontage(Weapon.GetMontage(direction));
		}

		public void OnAttackHitReceived(DamageArea.DamageHitInfo info)
		{
			// _bounceCoroutine = _montagePlayer.StartCoroutine(BounceCoroutine(info));
			var t = _montagePlayer.PlayableDirector.time;
			var max = _montagePlayer.PlayableDirector.playableAsset.duration;
			_montagePlayer.PlayMontage(Weapon.GetMontage(_attackDirection, true));
			// _montagePlayer.PlayableDirector.time = max - t;
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
		public void BeginBlock()
		{
			_animator.SetBool(IsBlockingHash, true);
		}
		public void EndBlock()
		{
			_animator.SetBool(IsBlockingHash, false);
		}
	}
}

