using FPSMeleeDemo.AI.BT;
using UnityEngine;

namespace FPSMeleeDemo.AI
{
    [CreateAssetMenu(menuName = "FPS/AI/Behaviours/Test")]
	public class TestAIBehaviour: AIBehaviour
	{
		private BehaviourTree _behaviourTree;

		public override void Begin()
		{
			_behaviourTree = new BehaviourTree(
				new BTLog { Message = "BT Start"},
				new BTWait { Duration = 2 },
				new BTLog { Message = "BT Completed!" }
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


