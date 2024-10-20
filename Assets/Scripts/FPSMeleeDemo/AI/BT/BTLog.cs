using UnityEngine;

namespace FPSMeleeDemo.AI.BT
{
	public class BTLog : BTAction
	{
		public string Message = "Hello, world";

		protected override void OnEntered()
		{
			Debug.Log("BTLog Enter");
		}

		protected override void OnExited()
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