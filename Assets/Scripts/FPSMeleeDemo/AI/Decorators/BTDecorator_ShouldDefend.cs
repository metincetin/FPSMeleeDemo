using FPSMeleeDemo.AI.BT;
using FPSMeleeDemo.Randomization;

namespace FPSMeleeDemo.AI.Decorators
{
	public class BTDecorator_ShouldDefend: BTDecorator
	{
		private readonly RandomizedValue _shouldDefendValue;
		private readonly float _requiredChanceForDefence;
		
		public BTDecorator_ShouldDefend(float requiredChanceForDefence)
		{
			var randomizedValue = new RandomizedValueWeighted();
			randomizedValue.SetWeight(.8f);
			_shouldDefendValue = randomizedValue;

			_requiredChanceForDefence = requiredChanceForDefence;
		}

		protected override bool ShouldRun => _shouldDefendValue.Check(_requiredChanceForDefence);
	}
}
