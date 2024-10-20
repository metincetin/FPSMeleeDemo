using UnityEngine;

namespace FPSMeleeDemo.Randomization
{
	public class TimedRandomizedValue: RandomizedValue
	{
		private readonly float _time;
		private float _lastControlTime;

		private float _randomValue;

		public TimedRandomizedValue(float time)
		{
			_time = time;
			_randomValue = Random.value;
		}
		
		public override float Get()
		{
			if (Time.time - _lastControlTime > _time)
			{
				_lastControlTime = Time.time;
				_randomValue = Random.value;
			}

			return _randomValue;
		}

	}
}