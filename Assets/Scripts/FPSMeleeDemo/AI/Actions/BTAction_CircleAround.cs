using FPSMeleeDemo.AI.BT;
using FPSMeleeDemo.Movement;
using FPSMeleeDemo.Randomization;
using UnityEngine;

namespace FPSMeleeDemo.AI.Actions
{
	public class BTAction_CircleAround : BTAction
	{
		public IMovement Movement;

		public Transform Target;
		public Transform Transform;

		private float _radius;

		private float _angle;

		private float _radiusMultiplier = 1f;

		private TimedRandomizedValue _approachingRandomizedValue = new TimedRandomizedValue(0.8f);

		public override void Enter()
		{
			var diff = Target.position - Transform.position;
			_angle = Vector3.Angle(diff, Vector3.back);
		}

		protected override NodeStatus OnRun()
		{
			var diff = Target.position - Transform.position;

			_radius = diff.magnitude;
			if (_radius > 6) return NodeStatus.Fail;
			_radius -= _approachingRandomizedValue.InRange(-3, 45) * Time.deltaTime;
			_radius = Mathf.Max(0.5f, _radius);

			_angle += 10 * Time.deltaTime;

			var targetFw = Quaternion.Euler(0, _angle, 0) * Vector3.forward;

			var targetPos = Target.position + targetFw * _radius;
			var targetPosDiff = targetPos - Transform.position;

			var clamped = Vector3.ClampMagnitude(targetPosDiff, 1);


			Movement.MovementInput = new Vector2(clamped.x, clamped.z);

			return NodeStatus.Running;
		}
	}
}