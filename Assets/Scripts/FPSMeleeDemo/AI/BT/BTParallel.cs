namespace FPSMeleeDemo.AI.BT
{
	public class BTParallel : BTComposite
	{
		public BTParallel(params BTNode[] nodes) : base(nodes){}
		protected override NodeStatus OnRun()
		{
			foreach (BTNode node in _traverser.Iterate())
			{
				node.Run();
			}
			return NodeStatus.Running;
		}
	}
}