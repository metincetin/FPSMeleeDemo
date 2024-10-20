using FPSMeleeDemo.AI.BT;
using FPSMeleeDemo.FPS;
using UnityEngine;

namespace FPSMeleeDemo.AI.Actions
{
	public class BTAction_Block: BTAction
	{
		private readonly BlockHandler _blockHandler;

		public float BlockDuration = 2f;
		private float _blockStartTime;
		

		public BTAction_Block(BlockHandler blockHandler)
		{
			_blockHandler = blockHandler;
		}
		protected override void OnEntered()
		{
			_blockHandler.BeginBlock();
			_blockStartTime = Time.time;
		}
		
		protected override void OnExited()
		{
			_blockHandler.EndBlock();
		}

		protected override NodeStatus OnRun()
		{
			if (Time.time - _blockStartTime > BlockDuration) return NodeStatus.Success;
			return NodeStatus.Running;
		}
	}
}