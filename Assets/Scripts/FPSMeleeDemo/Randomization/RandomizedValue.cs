using UnityEngine;

namespace FPSMeleeDemo.Randomization
{
	public class RandomizedValue
	{
		
		public T Select<T>(params T[] values)
		{
			int v = (int)InRange(0, values.Length - 1);
			return values[v];
		}
		
		public bool Check(float value)
		{
			return Get() < value;
		}
		
		public virtual float Get()
		{
			return Random.value;
		}
		
		
		public float InRange(float min, float max)
		{
			float t = Get();

			return Mathf.Lerp(min, max, t);
		}
	}
}