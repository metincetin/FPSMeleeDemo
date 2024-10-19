using UnityEngine;

namespace FPSMeleeDemo.AI.BT
{
	public class BTLog : BTAction
	{
		public string Message = "Hello, world";

		public override void Enter()
		{
			Debug.Log("BTLog Enter");
		}

		public override void Exit()
		{
			Debug.Log("BTLog Exit");
		}

		protected override NodeStatus OnRun()
		{
			Debug.Log(Message);
			return NodeStatus.Success;
		}
	}
}