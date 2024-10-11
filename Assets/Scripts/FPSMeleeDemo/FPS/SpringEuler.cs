using UnityEngine;

namespace FPSMeleeDemo.FPS
{
	// https://theorangeduck.com/page/spring-roll-call
	public static class SpringDamper
	{
		public static void SpringDamperExactStiffnessDamping(
			ref float x,
			ref float v,
			float x_goal,
			float v_goal,
			float stiffness,
			float damping,
			float dt,
			float eps = 1e-5f)
		{
			float g = x_goal;
			float q = v_goal;
			float s = stiffness;
			float d = damping;
			float c = g + (d * q) / (s + eps);
			float y = d / 2.0f;

			// Critically Damped
			if (Mathf.Abs(s - (d * d) / 4.0f) < eps)
			{
				float j0 = x - c;
				float j1 = v + j0 * y;

				float eydt = Mathf.Exp(-y * dt); // Equivalent to fast_negexp

				x = j0 * eydt + dt * j1 * eydt + c;
				v = -y * j0 * eydt - y * dt * j1 * eydt + j1 * eydt;
			}
			// Under Damped
			else if (s - (d * d) / 4.0f > 0.0f)
			{
				float w = Mathf.Sqrt(s - (d * d) / 4.0f);
				float j = Mathf.Sqrt((Mathf.Pow(v + y * (x - c), 2) / (w * w + eps)) + Mathf.Pow(x - c, 2));
				float p = Mathf.Atan((v + (x - c) * y) / (-(x - c) * w + eps)); // Equivalent to fast_atan

				j = (x - c) > 0.0f ? j : -j;

				float eydt = Mathf.Exp(-y * dt); // Equivalent to fast_negexp

				x = j * eydt * Mathf.Cos(w * dt + p) + c;
				v = -y * j * eydt * Mathf.Cos(w * dt + p) - w * j * eydt * Mathf.Sin(w * dt + p);
			}
			// Over Damped
			else if (s - (d * d) / 4.0f < 0.0f)
			{
				float y0 = (d + Mathf.Sqrt(d * d - 4 * s)) / 2.0f;
				float y1 = (d - Mathf.Sqrt(d * d - 4 * s)) / 2.0f;
				float j1 = (c * y0 - x * y0 - v) / (y1 - y0);
				float j0 = x - j1 - c;

				float ey0dt = Mathf.Exp(-y0 * dt); // Equivalent to fast_negexp
				float ey1dt = Mathf.Exp(-y1 * dt); // Equivalent to fast_negexp

				x = j0 * ey0dt + j1 * ey1dt + c;
				v = -y0 * j0 * ey0dt - y1 * j1 * ey1dt;
			}
		}
	}
}
