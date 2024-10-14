using UnityEngine;

namespace FPSMeleeDemo.Gameplay.BattleCharacters
{
    public class CharacterLocomotionAnimator
	{
		public Vector2 MovementInput;
        private Animator _animator;

		private readonly int MovementXHash = Animator.StringToHash("MovementX");
		private readonly int MovementYHash = Animator.StringToHash("MovementY");

		private float _xValue;
		private float _yValue;

		private float _xValueCurVel;
		private float _yValueCurVel;

		private const float SmoothTime = .2f;

        public CharacterLocomotionAnimator(Animator animator)
		{
			_animator = animator;
		}

		public void Update()
		{
			_xValue = Mathf.SmoothDamp(_xValue, MovementInput.x, ref _xValueCurVel, SmoothTime);
			_yValue = Mathf.SmoothDamp(_yValue, MovementInput.y, ref _yValueCurVel, SmoothTime);
			_animator.SetFloat(MovementXHash, _xValue);
			_animator.SetFloat(MovementYHash, _yValue);
		}
	}
}

