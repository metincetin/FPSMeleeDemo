using FPSMeleeDemo.AI.BT;
using UnityEngine;

namespace FPSMeleeDemo.AI.Decorators
{
	public class BTRandomSelector: BTComposite
	{
		protected override void OnEntered()
		{
			_traverser.SetIndex(Random.Range(0, _traverser.Length));
		}

		protected override NodeStatus OnRun()
		{
			return _traverser.Current.Run();
		}
	}
}