using FPSMeleeDemo.BattleSystem;
using UnityEngine;

namespace FPSMeleeDemo.Rigging
{
	public class IKTargetLook: MonoBehaviour
	{
		public Transform Target;
		private Animator _animator;
		private void Awake()
		{
			_animator = GetComponent<Animator>();
		}
		
		private void OnAnimatorIK(int layerIndex)
		{
			if (!Target) return;
			_animator.SetLookAtPosition(Target.position + Vector3.up);
			_animator.SetLookAtWeight(1, .8f, 1);
		}
	}
}