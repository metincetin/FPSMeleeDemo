using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPSMeleeDemo.FPS
{
	// cheap fix for FPS mouse look in humanoid characters
	public class IKMouseLook : MonoBehaviour
	{
		[SerializeField]
		private Transform _targetTransform;

		private Animator _animator;

		private void Awake()
		{
			_animator = GetComponent<Animator>();
		}

		private void OnAnimatorIK(int layerIndex)
		{
			_animator.SetLookAtWeight(1, 1, 0);
			_animator.SetLookAtPosition(_targetTransform.position + _targetTransform.forward * 10);
		}
	}
}
