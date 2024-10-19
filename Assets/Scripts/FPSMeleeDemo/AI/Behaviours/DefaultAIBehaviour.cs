using FPSMeleeDemo.AI.Actions;
using FPSMeleeDemo.AI.BT;
using FPSMeleeDemo.AI.Decorators;
using FPSMeleeDemo.FPS;
using FPSMeleeDemo.Gameplay;
using FPSMeleeDemo.Movement;
using UnityEngine;

namespace FPSMeleeDemo.AI.Behaviours
{
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