using UnityEngine;

/// https://gist.githubusercontent.com/sketchpunk/3568150a04b973430dfe8fd29bf470c8/raw/5b9d3a5bbcc1ca91b7cb770142ca7debc6b66e75/QuaterionSpring.js
namespace FPSMeleeDemo.FPS
{
	public class QuaternionSpring : MonoBehaviour
	{
		private Vector4 velocity;

		[SerializeField]
		private float stiffness;

		[SerializeField]
		private float damping;

		[SerializeField]
		private float limit;

		private Quaternion _currentRotation = Quaternion.identity;

		private float VelLenSqr()
		{
			return velocity.x * velocity.x + velocity.y * velocity.y + velocity.z * velocity.z + velocity.w * velocity.w;
		}

		// Harmonic oscillation
		public void OscillationStep(ref Quaternion cq, Quaternion target, float dt)
		{
			// Check when the spring is done
			float dot = Quaternion.Dot(cq, target);
			if (dot >= 0.9999f && VelLenSqr() < 0.000001f)
			{
				cq = target;
				return;
			}

			// Use the closest rotation
			Quaternion tq = dot < 0 ? new Quaternion(-target.x, -target.y, -target.z, -target.w) : target;

			// Update velocity
			velocity.x += (-stiffness * (cq.x - tq.x) - damping * velocity.x) * dt;
			velocity.y += (-stiffness * (cq.y - tq.y) - damping * velocity.y) * dt;
			velocity.z += (-stiffness * (cq.z - tq.z) - damping * velocity.z) * dt;
			velocity.w += (-stiffness * (cq.w - tq.w) - damping * velocity.w) * dt;

			// Update current quaternion
			cq.x += velocity.x * dt;
			cq.y += velocity.y * dt;
			cq.z += velocity.z * dt;
			cq.w += velocity.w * dt;

			cq.Normalize();
		}

		// Critically Damped Spring
		public void CriticallyStep(ref Quaternion cq, Quaternion target, float dt)
		{
			// Check when the spring is done
			float dot = Quaternion.Dot(cq, target);
			if (dot > limit){
				cq = Quaternion.Slerp(cq, target, limit);
			}
			//if (dot >= 0.9999f && VelLenSqr() < 0.000001f)
			//{
			//	cq = target;
			//	return;
			//}

			// Use the closest rotation
			Quaternion tq = dot < 0 ? new Quaternion(-target.x, -target.y, -target.z, -target.w) : target;

			// Calculate new velocity
			float dSqrDt = damping * damping * dt;
			float n2 = 1 + damping * dt;
			float n2Sqr = n2 * n2;

			velocity.x = (velocity.x - (cq.x - tq.x) * dSqrDt) / n2Sqr;
			velocity.y = (velocity.y - (cq.y - tq.y) * dSqrDt) / n2Sqr;
			velocity.z = (velocity.z - (cq.z - tq.z) * dSqrDt) / n2Sqr;
			velocity.w = (velocity.w - (cq.w - tq.w) * dSqrDt) / n2Sqr;

			// Update current quaternion
			cq.x += velocity.x * dt;
			cq.y += velocity.y * dt;
			cq.z += velocity.z * dt;
			cq.w += velocity.w * dt;

			cq.Normalize();
		}

		private void LateUpdate()
		{
			var targetRot = transform.parent.rotation;

			CriticallyStep(ref _currentRotation, targetRot, Time.deltaTime);

			transform.rotation = _currentRotation;
		}
	}
}
