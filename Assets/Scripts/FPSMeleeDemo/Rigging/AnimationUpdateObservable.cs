using System;
using UnityEngine;

namespace FPSMeleeDemo.PhysicsModule
{
    public class AnimationUpdateObservable : MonoBehaviour
	{
		public event Action UpdateReceived;

		private void OnAnimatorMove()
		{
			UpdateReceived?.Invoke();
		}
	}
}

