using FPSMeleeDemo.AI.BT;
using UnityEngine;

namespace FPSMeleeDemo.AI.Decorators
{
	public class BTTimedActionContainer : BTComposite
	{
		public float MaxTime;

		private float _startTime;

		public override void Enter()
		{
			_startTime = Time.time;
		}

		protected override NodeStatus OnRun()
		{
			if (Time.time - _startTime > MaxTime) return NodeStatus.Success;
			return _traverser.Current.Run();
		}
	}
}