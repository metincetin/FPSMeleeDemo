using FPSMeleeDemo.AI.BT;
using FPSMeleeDemo.FPS;
using UnityEngine;

namespace FPSMeleeDemo.AI.Decorators
{
	public class BTDecorator_IsTargetAttacking: BTDecorator
	{
		private IAttacker _attacker;
		
		public BTDecorator_IsTargetAttacking(IAttacker attacker)
		{
			_attacker = attacker;
		}
		
		protected override bool ShouldRun 
		{
			get
			{
				if (_attacker is not IAttackStateProvider attackStateProvider) return false;
				return attackStateProvider.IsAttacking;
			}
		}
	}
}