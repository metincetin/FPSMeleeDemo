using FPSMeleeDemo.AI.BT;
using FPSMeleeDemo.FPS;
using FPSMeleeDemo.Gameplay;
using FPSMeleeDemo.Movement;
using FPSMeleeDemo.PhysicsModule;
using UnityEngine;

namespace FPSMeleeDemo.AI
{
	public class RandomizedValue
	{
		public virtual float Get()
		{
			return Random.value;
		}
	}
	public class BTDecorator_DistanceCheck : BTDecorator
	{
		public float RequiredDistance;
		public Transform Transform;
		public Transform Target;
		protected override bool ShouldRun => Vector3.Distance(Transform.position, Target.position) < RequiredDistance;
	}

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

	[CreateAssetMenu(menuName = "FPS/AI/Behaviours/Default")]
	public class DefaultAIBehaviour : AIBehaviour
	{
		private BehaviourTree _behaviourTree;

		public override void Begin()
		{
			var battleCharacter = Owner.GetComponent<IBattleCharacter>();
			var movement = Owner.GetComponent<IMovement>();

			var attacker = Owner.GetComponent<IAttacker>();

			var other = BattleInstance.GetOther(battleCharacter);
			var otherTransform = (other as MonoBehaviour).transform;

			_behaviourTree = new BehaviourTree(
				new BTLog { Message = "BT Start" },
				new BTWait { Duration = 2 },
				new BTParallel(
					new BTSequence(new BTNode[]
					{
						new BTAction_ApproachPlayer
							{ Target = otherTransform, Movement = movement, Transform = Owner.transform },
						new BTAction_CircleAround
							{ Target = otherTransform, Movement = movement, Transform = Owner.transform },
					}),
					new BTAction_Attack
					{
						Decorators = new[]
						{
							new BTDecorator_DistanceCheck { RequiredDistance = 2, Target = otherTransform, Transform = Owner.transform }
						},
						Attacker = attacker, Target = otherTransform
					}
				)
			);

			_behaviourTree.Begin();
		}

		public override void End()
		{
			_behaviourTree.End();
		}

		public override void Update()
		{
			_behaviourTree.Update();
		}
	}
}