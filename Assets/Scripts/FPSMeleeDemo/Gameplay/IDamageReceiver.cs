using System;

namespace FPSMeleeDemo.Gameplay
{
    public interface IDamageReceiver
	{
		public void ApplyDamage(DamageObject damage);
		public event Action<DamageObject> DamageReceived;
	}
}

