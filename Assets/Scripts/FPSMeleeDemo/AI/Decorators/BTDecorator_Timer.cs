using FPSMeleeDemo.AI.BT;
using UnityEngine;

namespace FPSMeleeDemo.AI.Decorators
{
	/// <summary>
	/// Fails if node is executed for specified time
	/// </summary>
	public class BTDecorator_Timer: BTDecorator
	{
		private float _beginTime;
		private readonly float _time;

		public BTDecorator_Timer(float time)
		{
			_time = time;
		}

		public override void OnNodeEntered()
		{
			_beginTime = Time.time;
		}
		
		protected override bool ShouldRun => Time.time - _beginTime <= _time;
	}
}