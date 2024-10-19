using FPSMeleeDemo.AI.BT;
using UnityEngine;

namespace FPSMeleeDemo.AI.Decorators
{
	public class BTDecorator_DistanceCheck : BTDecorator
	{
		public float RequiredDistance;
		public Transform Transform;
		public Transform Target;
		protected override bool ShouldRun => Vector3.Distance(Transform.position, Target.position) < RequiredDistance;
	}
}