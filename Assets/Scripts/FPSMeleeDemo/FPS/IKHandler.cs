using UnityEngine;

namespace FPSMeleeDemo.FPS
{
    public class IKHandler : MonoBehaviour
	{
		private Animator _animator;

		public Vector3 RightHandGoal { get; set; }
		public float RightHandWeight { get; set; } = 0;

		private void Awake()
		{
			_animator = GetComponentInChildren<Animator>();
		}

		private void OnAnimatorIK(int layerIndex)
		{
			_animator.SetIKPositionWeight(AvatarIKGoal.RightHand, RightHandWeight);
			_animator.SetIKPosition(AvatarIKGoal.RightHand, RightHandGoal);
		}
	}
}


