using FPSMeleeDemo.AI.BT;
using FPSMeleeDemo.Movement;
using FPSMeleeDemo.Randomization;
using UnityEngine;

namespace FPSMeleeDemo.AI.Actions
{
	public class BTAction_Dash : BTAction
	{
		public Transform Target;
		private readonly IDash _dasher;
		private readonly Transform _owner;

		RandomizedValue _dashDirectionSelector = new RandomizedValue();
		
		public BTAction_Dash(Transform owner, IDash dasher)
		{
			_owner = owner;
			_dasher = dasher;
		}

		protected override NodeStatus OnRun()
		{
			var ownerTransform = _owner.transform;
			var dashDirection = _dashDirectionSelector.Select<Vector3>(ownerTransform.right, -ownerTransform.right, -ownerTransform.forward);
			_dasher.Dash(dashDirection);
			return NodeStatus.Success;
		}
	}
}