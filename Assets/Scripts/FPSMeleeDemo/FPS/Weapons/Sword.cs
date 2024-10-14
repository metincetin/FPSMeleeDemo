using UnityEngine;
using UnityEngine.Playables;

namespace FPSMeleeDemo.Data
{
	[CreateAssetMenu(menuName = "FPS/Weapons/Sword")]
    public class Sword : Weapon
    {
		[System.Serializable]
		private class CardinalAttackMontagePair
		{
			public PlayableAsset Left;
			public PlayableAsset Right;
			public PlayableAsset Up;
			public PlayableAsset Down;
		}

		[SerializeField]
		private CardinalAttackMontagePair _montagePairs;

		[SerializeField]
		private CardinalAttackMontagePair _reversedPairs;

        public override PlayableAsset GetMontage(CardinalDirection direction, bool reversed = false)
        {
			var pairs = reversed ? _reversedPairs : _montagePairs;

			switch (direction)
			{
				case CardinalDirection.West:
					return pairs.Left;
				case CardinalDirection.East:
					return pairs.Right;
				case CardinalDirection.North:
					return pairs.Up;
				case CardinalDirection.South:
					return pairs.Down;
			}
			return pairs.Up;
        }
    }
}
