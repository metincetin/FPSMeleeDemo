using UnityEngine;

namespace FPSMeleeDemo.AI.BT
{
	public class BTWait : BTAction
	{
		public float Duration;
		private float _startTime;

		protected override void OnEntered()
		{
			_startTime = Time.time;
		}

		protected override NodeStatus OnRun()
		{
			if (Time.time - _startTime > Duration) return NodeStatus.Success;
			return NodeStatus.Running;
		}
	}
}