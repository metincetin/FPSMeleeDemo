using System;
using UnityEngine;

namespace FPSMeleeDemo.Rigging
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

