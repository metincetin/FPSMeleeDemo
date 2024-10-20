using FPSMeleeDemo.FPS;
using UnityEngine;
using UnityEngine.Playables;

namespace FPSMeleeDemo.Data
{
	public abstract class Weapon : ScriptableObject, IWeapon
	{
		[SerializeField]
		private GameObject _graphics;
		public GameObject Graphics => _graphics;

		public abstract PlayableAsset GetFPSMontage(CardinalDirection direction, bool reversed = false);
		public abstract PlayableAsset GetThirdPersonMontage(CardinalDirection direction, bool reversed = false);
	}
}

