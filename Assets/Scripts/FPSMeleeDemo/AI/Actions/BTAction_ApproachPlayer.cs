using FPSMeleeDemo.AI.BT;
using FPSMeleeDemo.Movement;
using UnityEngine;

namespace FPSMeleeDemo.AI.Actions
{
	public class BTAction_ApproachPlayer : BTAction
	{
		public Transform Target;

		public float AllowedDistance = 5;
		public IMovement Movement;
		public Transform Transform;

		private float GetDistance() => Vector3.Distance(Transform.position, Target.position);

		protected override NodeStatus OnRun()
		{
			var dist = GetDistance();
			if (dist <= AllowedDistance) return NodeStatus.Success;

			var dir = (Target.position - Transform.position);
			var clamped = Vector3.ClampMagnitude(dir, 1);

			Movement.MovementInput = new Vector2(clamped.x, clamped.z);

			return NodeStatus.Running;
		}
	}
}