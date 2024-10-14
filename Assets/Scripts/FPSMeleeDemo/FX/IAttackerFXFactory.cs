using System.Collections;
using System.Collections.Generic;
using FPSMeleeDemo.FPS;

namespace FPSMeleeDemo.FX
{
    public interface IAttackerFXFactory
	{
		public void CreateHitEffect(DamageArea.DamageHitInfo info);
	}
}
