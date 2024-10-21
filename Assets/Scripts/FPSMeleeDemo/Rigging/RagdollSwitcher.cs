using UnityEngine;

namespace FPSMeleeDemo.Rigging
{
	public class RagdollSwitcher: MonoBehaviour
	{
		private Rigidbody[] _parts;
		private Animator _animator;

		private void Awake()
		{
			_parts = GetComponentsInChildren<Rigidbody>();
			_animator = GetComponent<Animator>();
		}
		
		public void SetRagdoll()
		{
			_animator.enabled = false;
			foreach(var p in _parts)
			{
				p.isKinematic = false;
			}
		}
	}
}