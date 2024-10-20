using System.Timers;
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

		private float _angle;

		private float _radiusMultiplier = 1f;

		private TimedRandomizedValue _approachingRandomizedValue = new TimedRandomizedValue(0.8f);
		private TimedRandomizedValue _directionRandomizedValue = new TimedRandomizedValue(1.8f);
		
		private Timer _radiusAdjustmentTimer;

		protected override void OnEntered()
		{
			var diff = Target.position - Transform.position;
			_angle = Vector3.Angle(diff, Vector3.back);

			_radiusAdjustmentTimer = new Timer(5000);
			_radiusAdjustmentTimer.Elapsed += OnRadiusAdjustmentTimerElapsed;
			_radiusAdjustmentTimer.Start();
		}

		protected override void OnExited()
		{
			_radiusAdjustmentTimer.Stop();
		}

		private void OnRadiusAdjustmentTimerElapsed(object sender, ElapsedEventArgs e)
		{
			_radiusMultiplier = 1;
		}

		protected override NodeStatus OnRun()
		{
			var diff = Target.position - Transform.position;

			float radius = diff.magnitude;
			
			if (radius > 6) return NodeStatus.Fail;

			_radiusMultiplier += _approachingRandomizedValue.InRange(-.1f, .04f) * Time.deltaTime;
			
			radius *= _radiusMultiplier;
			
			radius = Mathf.Max(1.5f, radius);

			_angle += 10 * Time.deltaTime * Mathf.Sign(_directionRandomizedValue.InRange(-1, 1));

			var targetFw = Quaternion.Euler(0, _angle, 0) * Vector3.forward;

			var targetPos = Target.position + targetFw * radius;
			var targetPosDiff = targetPos - Transform.position;

			var clamped = Vector3.ClampMagnitude(targetPosDiff, 1);


			Movement.MovementInput = new Vector2(clamped.x, clamped.z);

			return NodeStatus.Running;
		}
	}
}