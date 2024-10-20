using FPSMeleeDemo.Data;
using FPSMeleeDemo.Montage;
using UnityEngine.Playables;

namespace FPSMeleeDemo.FPS
{
    public class AIAttackAnimationHandler : BaseAttackAnimationHandler
    {
        public AIAttackAnimationHandler(MontagePlayer montagePlayer) : base(montagePlayer)
        {
        }

        protected override PlayableAsset RequestMontage(CardinalDirection attackDirection, bool reversed = false)
        {
			return Weapon.GetThirdPersonMontage(attackDirection, reversed);
        }
    }
}


