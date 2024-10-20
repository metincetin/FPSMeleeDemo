namespace FPSMeleeDemo.AI.BT
{
	public abstract class BTDecorator
	{
		public virtual void OnNodeEntered(){}
		public virtual void OnNodeExited(){}
			
		public bool Invert { get; set; }
		public bool Enabled { get; set; } = true;
		protected abstract bool ShouldRun { get; }
		public bool Test 
		{
			get
			{
				if (Enabled) return Invert ? !ShouldRun : ShouldRun;
				return true;
			}
		} 
	}
}