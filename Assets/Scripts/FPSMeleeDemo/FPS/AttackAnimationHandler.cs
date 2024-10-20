using FPSMeleeDemo.Data;

namespace FPSMeleeDemo.FPS
{
	public abstract class AttackAnimationHandler
	{
		public Weapon Weapon { get; set; }
		public abstract void Play(CardinalDirection direction);
        public abstract void OnAttackHitReceived(DamageArea.DamageHitInfo info);

        public bool IsAttacking { get; set; }

        public abstract void BeginBlock();
        public abstract void EndBlock();
	}
}
