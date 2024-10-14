using FPSMeleeDemo.Data.Sets;
using FPSMeleeDemo.FPS;
using UnityEngine;
using UnityEngine.Pool;

namespace FPSMeleeDemo.FX
{
    [CreateAssetMenu(menuName = "FPS/VFX/AttackerFXFactory")]
	public class AttackerFXFactory : ScriptableObject, IAttackerFXFactory
	{

		[SerializeField]
		private MaterialGameObjectSet _particleSet;

		[SerializeField]
		private MaterialGameObjectSet _decalSet;

		[SerializeField]
		private float _releaseDuration = 4f;

		public void CreateHitEffect(DamageArea.DamageHitInfo info)
		{
			var p = info.Point;
			var decalInst = UnityEngine.Object.Instantiate(_decalSet.GetOrDefault(info.Collider.sharedMaterial), p + info.Normal * 0.01f, Quaternion.identity);
			decalInst.transform.forward = -info.Normal;
			Debug.Log(Mathf.Atan2(info.RelativeVelocity.z, info.RelativeVelocity.x));
			decalInst.transform.Rotate(info.Normal, Mathf.Atan2(info.RelativeVelocity.z, info.RelativeVelocity.x), Space.World);
			Destroy(decalInst, _releaseDuration);

			var particle = UnityEngine.Object.Instantiate(_particleSet.GetOrDefault(info.Collider.sharedMaterial), p, Quaternion.identity);
			particle.transform.forward = -info.RelativeVelocity.normalized;

			Destroy(particle, _releaseDuration);
		}
	}
}

