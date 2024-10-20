using FPSMeleeDemo.AI.BT;
using FPSMeleeDemo.AI.Decorators;
using FPSMeleeDemo.FPS;
using FPSMeleeDemo.Movement;
using FPSMeleeDemo.Randomization;
using UnityEngine;

namespace FPSMeleeDemo.AI.Actions
{
	public class BTSequence_Defend: BTRandomSelector
	{
        private GameObject _owner;

		private Transform _targetTransform;
        private IDash _dasher;

		private BlockHandler _blockHandler;
		private readonly BTDecorator_ShouldDefend _shouldDefendDecorator;
		
		public BTSequence_Defend(GameObject owner, IAttacker target)
		{
			_owner = owner;
			_targetTransform = (target as MonoBehaviour).transform;

			_blockHandler = owner.GetComponent<IAttacker>().BlockHandler;
			
			_shouldDefendDecorator = new BTDecorator_ShouldDefend(.4f);
			
			_dasher = _owner.GetComponent<IDash>();


			Decorators = new BTDecorator[]
			{
				//new BTDecorator_Timer(1f),
				new BTDecorator_IsTargetAttacking(target),
				_shouldDefendDecorator
			};

			_traverser = new NodeTraverser(
				new BTAction_Block(_blockHandler),
				new BTAction_Dash(owner.transform,_dasher){Target = _targetTransform}
			);
		}
	}
}
