namespace FPSMeleeDemo.AI.BT
{
	public abstract class BTComposite : BTNode, INodeTraverser
	{
		protected NodeTraverser _traverser;

		public BTComposite(params BTNode[] nodes)
		{
			_traverser = new NodeTraverser(nodes);
		}

		public override void Enter()
		{
			_traverser.Begin();
		}


		public BTNode Current => _traverser.Current;

		public bool HasNext => _traverser.HasNext;

		public void Restart()
		{
			_traverser.Restart();
		}

		public void Traverse()
		{
			_traverser.Traverse();
		}
	}
}