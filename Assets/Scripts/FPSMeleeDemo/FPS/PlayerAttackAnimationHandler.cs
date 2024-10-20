using System;
using FPSMeleeDemo.Data;
using FPSMeleeDemo.Montage;
using UnityEngine;
using UnityEngine.Playables;

namespace FPSMeleeDemo.FPS
{
    public class PlayerAttackAnimationHandler: BaseAttackAnimationHandler
	{
        public PlayerAttackAnimationHandler(MontagePlayer montagePlayer) : base(montagePlayer)
        {
        }

        protected override PlayableAsset RequestMontage(CardinalDirection attackDirection, bool reversed = false)
        {
			return Weapon.GetFPSMontage(attackDirection, reversed);
        }
	}
}

