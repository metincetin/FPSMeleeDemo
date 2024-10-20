using System.Collections;
using FPSMeleeDemo.Data;
using FPSMeleeDemo.Montage;
using UnityEngine;
using UnityEngine.Playables;

namespace FPSMeleeDemo.FPS
{
    public abstract class BaseAttackAnimationHandler : AttackAnimationHandler
	{
		private MontagePlayer _montagePlayer;
		private Animator _animator;
		private Coroutine _bounceCoroutine;


		private CardinalDirection _attackDirection;

		private readonly int IsBlockingHash = Animator.StringToHash("IsBlocking");


		public BaseAttackAnimationHandler(MontagePlayer montagePlayer)
		{
			this._montagePlayer = montagePlayer;
			this._animator = montagePlayer.GetComponentInChildren<Animator>();
		}

		protected abstract PlayableAsset RequestMontage(CardinalDirection attackDirection, bool reversed = false); 

		public void SetAttackDirection(CardinalDirection direction)
		{
			_attackDirection = direction;
		}
		public override void Play(CardinalDirection attackDirection)
		{
			if (IsAttacking) return;
			IsAttacking = true;
			_attackDirection = attackDirection;
			var handle = _montagePlayer.PlayMontageAndWaitEvent(RequestMontage(_attackDirection), OnMontageEvent);
			handle.Ended += OnMontageEnded;
		}


		// once montage ends, make sure to reset IsAttacking anyways, in case someone forgets to add the event..
        private void OnMontageEnded()
        {
			IsAttacking = false;
        }

        private void OnMontageEvent(MontagePlayer.MontageEvent @event)
        {
			IsAttacking = false;
        }

		public override void OnAttackHitReceived(DamageArea.DamageHitInfo info)
		{
			// _bounceCoroutine = _montagePlayer.StartCoroutine(BounceCoroutine(info));
			var t = _montagePlayer.PlayableDirector.time;
			var max = _montagePlayer.PlayableDirector.playableAsset.duration;
			_montagePlayer.PlayMontage(RequestMontage(_attackDirection, true));
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
		public override void BeginBlock()
		{
			_animator.SetBool(IsBlockingHash, true);
		}
		public override void EndBlock()
		{
			_animator.SetBool(IsBlockingHash, false);
		}
	}
}


