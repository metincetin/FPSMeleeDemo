using UnityEngine;

namespace FPSMeleeDemo.Data
{
    public enum CardinalDirection
	{
		West,
		East,
		North,
		South,
		NortWest,
		NortEast,
		SouthWest,
		SouthEast,
	}
    public static class CardinalDirectionExtensions
	{
		public static Vector2 ToVector(CardinalDirection cardinalDirection)
		{
			switch (cardinalDirection)
			{
				case CardinalDirection.West:
					return Vector2.left;
				case CardinalDirection.East:
					return Vector2.right;
				case CardinalDirection.North:
					return Vector2.up;
				case CardinalDirection.South:
					return Vector2.down;
			}

			return Vector2.right;
		}
		
		public static CardinalDirection ToCardinal(this Vector2 value)
		{
			if (Mathf.Abs(value.x) > Mathf.Abs(value.y))
			{
				return value.x > 0 ? CardinalDirection.East : CardinalDirection.West;
			}

			return value.y > 0 ? CardinalDirection.North: CardinalDirection.South;
		}
	}
}
