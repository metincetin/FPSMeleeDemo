using UnityEngine;

namespace FPSMeleeDemo.Movement
{
    public class RootMotionVelocityManipulator : IMovementVelocityManipulator
    {
		private readonly RootMotionController _controller;

		public RootMotionVelocityManipulator(RootMotionController controller)
		{
			_controller = controller;
		}

        public Vector3 GetVelocity()
        {
			return _controller.Velocity;
        }
    }
}

