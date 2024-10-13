using FPSMeleeDemo.Montage;
using UnityEngine;
using UnityEngine.Playables;

namespace FPSMeleeDemo.FPS
{
    public class AttackAnimationHandler
	{
		private MontagePlayer _montagePlayer;

        public AttackAnimationHandler(MontagePlayer montagePlayer)
		{
			this._montagePlayer = montagePlayer;
		}

		public void Play(PlayableAsset asset)
		{
			_montagePlayer.PlayMontage(asset);
		}
	}
}

