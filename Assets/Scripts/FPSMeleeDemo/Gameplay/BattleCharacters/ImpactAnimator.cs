using UnityEngine;

namespace FPSMeleeDemo.Gameplay.BattleCharacters
{
	public class ImpactAnimator
	{
		private readonly Animator _animator;

		public ImpactAnimator(Animator animator)
		{
			_animator = animator;
		}

		public void PlayHit(Vector3 hitVector)
		{
			hitVector = _animator.transform.TransformDirection(hitVector);
			_animator.SetTrigger("Hit");
			var horizontal = 0f;
			if (Mathf.Abs(hitVector.x) > Mathf.Abs(hitVector.z))
			{
				horizontal = hitVector.x;
			}
			else
			{
				horizontal = hitVector.z;
			}

			_animator.SetFloat("HitDirectionX", horizontal);
			_animator.SetFloat("HitDirectionY", hitVector.y);
		}
	}
}