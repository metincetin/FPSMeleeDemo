namespace FPSMeleeDemo.AI.BT
{
	public abstract class BTNode
	{
		public BTDecorator[] Decorators { get; set; }

		public virtual void Enter() { }
		public virtual void Exit() { }
		
		public BTNode(params BTDecorator[] decorators)
		{
			Decorators = decorators;
		}

		public bool GetDecoratorResult()
		{
			if (Decorators == null) return true;
			foreach (var d in Decorators)
			{
				if (!d.Test) return false;
			}
			return true;
		}

		public NodeStatus Run()
		{
			if (!GetDecoratorResult()) return NodeStatus.Fail;


			return OnRun();
		}
		protected abstract NodeStatus OnRun();
	}
}