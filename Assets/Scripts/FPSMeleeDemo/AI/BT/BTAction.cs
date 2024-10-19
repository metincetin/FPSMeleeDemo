namespace FPSMeleeDemo.AI.BT
{
	public abstract class BTAction : BTNode
	{
		protected BTAction(params BTDecorator[] decorators) : base(decorators){}
	}
}