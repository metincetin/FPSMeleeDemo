using FPSMeleeDemo.Data.Sets;
using FPSMeleeDemo.FPS;
using UnityEngine;

namespace FPSMeleeDemo.FX
{
    [CreateAssetMenu(menuName = "FPS/VFX/AttackerFXFactory")]
	public class AttackerFXFactory : ScriptableObject, IAttackerFXFactory
	{

		[SerializeField]
		private MaterialGameObjectSet _particleSet;

		public void CreateHitEffect(DamageArea.DamageHitInfo info)
		{
			var p = info.Point;

			// var decalInst = UnityEngine.Object.Instantiate(_decal, p, Quaternion.identity);
			// decalInst.transform.up = info.Normal;

			var particle = UnityEngine.Object.Instantiate(_particleSet.GetOrDefault(info.Collider.sharedMaterial), p, Quaternion.identity);
			particle.transform.forward = -info.RelativeVelocity.normalized;

		}
	}
}

