using System;

namespace FPSMeleeDemo.Gameplay
{
    public interface IDamageCauser
	{
        public event Action<IDamageReceiver, DamageObject> DamageCaused;
	}
}

