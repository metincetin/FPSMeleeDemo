using System;

namespace FPSMeleeDemo.Gameplay
{
	public interface IDependant<T>
	{
		public T Value { get; set; }
	}
	public struct Attributes
	{
		public float Health { get; set; }
		public float MaxHealth { get; set; }
		public float BlockHealth { get; set; }
		public float Damage { get; set; }
		public float Defence { get; set; }
	}
}

