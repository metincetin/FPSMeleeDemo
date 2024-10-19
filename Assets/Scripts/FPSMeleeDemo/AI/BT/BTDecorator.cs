namespace FPSMeleeDemo.AI.BT
{
	public abstract class BTDecorator
	{
		public bool Invert { get; }
		protected abstract bool ShouldRun { get; }
		public bool Test => Invert ? !ShouldRun : ShouldRun;
	}
}