using FPSMeleeDemo.AI.BT;
using FPSMeleeDemo.AI.Decorators;
using FPSMeleeDemo.FPS;
using FPSMeleeDemo.Movement;
using FPSMeleeDemo.Randomization;
using UnityEngine;

namespace FPSMeleeDemo.AI.Actions
{
	public class BTAction_Defend: BTAction
	{
		private TimedRandomizedValue _defenceSelector;

		private RandomizedValue _dashDirection;

        private GameObject _owner;

		private Transform _targetTransform;
        private IDash _dasher;

		private BlockHandler _blockHandler;

        private enum DefenceType
		{
			Dash,
			Block
		}
		
		public BTAction_Defend(GameObject owner, IAttacker target)
		{
			_owner = owner;

			_targetTransform = (target as MonoBehaviour).transform;

			_defenceSelector = new TimedRandomizedValue(3);

			_blockHandler = owner.GetComponent<IAttacker>().BlockHandler;


			Decorators = new BTDecorator[]
			{
				new BTDecorator_Timer(1f),
				new BTDecorator_IsTargetAttacking(target),
				new BTDecorator_ShouldDefend(.4f),
			};
		}

		protected override void OnEntered()
		{
			_dasher = _owner.GetComponent<IDash>();
		}

		protected override NodeStatus OnRun()
		{
			var defenceType = _defenceSelector.Select<DefenceType>(DefenceType.Dash, DefenceType.Block);

			_blockHandler.EndBlock();

			switch(defenceType)
			{
				case DefenceType.Dash:
					var ownerTransform = _owner.transform;
					var dashDirection = _dashDirection.Select<Vector3>(ownerTransform.right, -ownerTransform.right, -ownerTransform.forward);
					_dasher.Dash(dashDirection);
					break;
				case DefenceType.Block:
					_blockHandler.BeginBlock();
					break;
			}

			return NodeStatus.Running;
		}
	}
}
