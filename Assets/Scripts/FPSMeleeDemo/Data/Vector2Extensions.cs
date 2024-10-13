
using UnityEngine;

namespace FPSMeleeDemo.Data
{
    public static class Vector2Extensions
	{
		public static CardinalDirection ToCardinal(this Vector2 value)
		{
			if (Mathf.Abs(value.x) > Mathf.Abs(value.y))
			{
				return value.x > 0 ? CardinalDirection.East : CardinalDirection.West;
			}
			else
			{
				return value.y > 0 ? CardinalDirection.North: CardinalDirection.South;
			}
		}
	}
}

