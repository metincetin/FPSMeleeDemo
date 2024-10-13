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

        public override PlayableAsset GetMontage(CardinalDirection direction)
        {
			switch (direction)
			{
				case CardinalDirection.West:
					return _montagePairs.Left;
				case CardinalDirection.East:
					return _montagePairs.Right;
				case CardinalDirection.North:
					return _montagePairs.Up;
				case CardinalDirection.South:
					return _montagePairs.Down;
			}
			return _montagePairs.Up;
        }
    }
}
