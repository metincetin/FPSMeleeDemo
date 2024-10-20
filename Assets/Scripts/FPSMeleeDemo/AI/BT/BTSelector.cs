namespace FPSMeleeDemo.AI.BT
{
	public class BTSelector : BTComposite
	{
		public BTSelector (params BTNode[] nodes): base(nodes){}

		protected override NodeStatus OnRun()
		{
			var result = Current.Run();
			if (result != NodeStatus.Running && !HasNext) return result;

			if (result == NodeStatus.Fail)
			{
				Traverse();
				return NodeStatus.Running;
			}

			return result;
		}
	}
}