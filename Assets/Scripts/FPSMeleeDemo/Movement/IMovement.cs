using UnityEngine;

namespace FPSMeleeDemo.Movement
{
    public interface IMovement
	{
		public void Jump();
		public Vector2 MovementInput { get; set; }
		public Vector3 Velocity { get; }
		public bool IsGrounded { get; }
	}
}

