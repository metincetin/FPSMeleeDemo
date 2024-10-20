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
		private CardinalAttackMontagePair _fpsMontagePairs;

		[SerializeField]
		private CardinalAttackMontagePair _fpsReversedPairs;

		[SerializeField]
		private CardinalAttackMontagePair _thirdPersonMontagePairs;

		[SerializeField]
		private CardinalAttackMontagePair _thirdPersonReversedPairs;

        public override PlayableAsset GetThirdPersonMontage(CardinalDirection direction, bool reversed = false)
		{
			var pairs = reversed ? _thirdPersonReversedPairs: _thirdPersonMontagePairs;

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

        public override PlayableAsset GetFPSMontage(CardinalDirection direction, bool reversed = false)
        {
			var pairs = reversed ? _fpsReversedPairs : _fpsMontagePairs;

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
