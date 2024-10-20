using FPSMeleeDemo.AI.BT;
using UnityEngine.UIElements;

namespace FPSMeleeDemo.AI.Decorators
{
	public class BTRepeater : BTComposite
	{
		public BTRepeater(params BTNode[] nodes) : base(nodes){}
		protected override NodeStatus OnRun()
		{
			var result = _traverser.Current.Run();
			if (result != NodeStatus.Running)
			{
				Restart();
			}

			return NodeStatus.Running;
		}
	}
}