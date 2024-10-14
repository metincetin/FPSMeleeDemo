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

		public abstract PlayableAsset GetMontage(CardinalDirection direction, bool reversed = false);
	}
}

