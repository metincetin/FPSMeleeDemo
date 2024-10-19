namespace FPSMeleeDemo.AI.BT
{
	public class BTSequence : BTComposite
	{
		public BTSequence(params BTNode[] nodes): base(nodes){}

		protected override NodeStatus OnRun()
		{
			var result = Current.Run();
			if (result == NodeStatus.Success)
			{
				if (_traverser.HasNext) Traverse();
				else return NodeStatus.Success;
			}

			return result;
		}
	}
}