using UnityEngine;

namespace FPSMeleeDemo.Randomization
{
	public class RandomizedValue
	{
		public virtual float Get()
		{
			return Random.value;
		}
	}
}