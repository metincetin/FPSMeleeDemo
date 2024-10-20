namespace FPSMeleeDemo.AI.BT
{
	public class BTParallel : BTComposite
	{
		public BTParallel(params BTNode[] nodes) : base(nodes){}
		protected override NodeStatus OnRun()
		{
			foreach (BTNode node in _traverser.Iterate())
			{
				var childResult = node.Run();
				//if (childResult == NodeStatus.Fail) Restart();
			}
			return NodeStatus.Running;
		}
	}
}