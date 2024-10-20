using UnityEngine;

namespace FPSMeleeDemo.Randomization
{
	public class RandomizedValueWeighted : RandomizedValue
	{
		private float _weight;
		public void SetWeight(float weight) => _weight = weight;

		public override float Get()
		{
			return Mathf.Pow(base.Get(), _weight);
		}
	}
}