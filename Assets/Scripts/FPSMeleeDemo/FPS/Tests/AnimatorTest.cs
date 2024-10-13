using UnityEngine;

namespace FPSMeleeDemo.FPS.Tests
{
    public class AnimatorTest: MonoBehaviour
	{
		private void Update()
		{
			var animator = GetComponent<Animator>();

			animator.Update(Time.deltaTime);
		}
	}
}

