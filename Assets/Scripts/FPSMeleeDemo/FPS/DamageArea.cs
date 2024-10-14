using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPSMeleeDemo.FPS
{
	public class DamageArea : MonoBehaviour
	{
		public struct DamageHitInfo
		{
			public Vector3 Point;
			public Rigidbody Rigidbody;
			public Collider Collider;
			public Vector3 RelativeVelocity;
            public Vector3 Normal;
        }

		[System.Serializable]
		private class DamagePoint
		{
			public Vector3 Position;
			public Vector3 Normal;

			public Vector3 PreviousPosition { get; set; }
		}

		[SerializeField]
		private DamagePoint[] _damagePoints;

		private Rigidbody _rigidbody;
		private bool _awaitingDamaging;

		private HashSet<Collider> _ignores = new(5);
		private HashSet<Collider> _hits = new(10);

		public event Action<DamageHitInfo> HitReceived;

		private void Awake()
		{
			_rigidbody = GetComponent<Rigidbody>();
		}

		public void Begin()
		{
			Debug.Log("Begin");
			_awaitingDamaging = true;
			ResetPreviousPositions();
			_hits.Clear();
		}

		public void End()
		{
			_awaitingDamaging = false;
		}

		private void OnDrawGizmosSelected()
		{
			foreach (var p in _damagePoints)
			{
				var position = transform.TransformPoint(p.Position);
				Gizmos.color = _awaitingDamaging ? Color.green : Color.red;
				Gizmos.DrawCube(position, Vector3.one * 0.02f);
				Debug.DrawRay(position, transform.TransformDirection(p.Normal.normalized), Gizmos.color);
				Gizmos.color = Color.white;
			}
		}

		public void AddIgnore(Collider target)
		{
			_ignores.Add(target);
		}

		private void ResetPreviousPositions()
		{
			foreach(var p in _damagePoints)
			{
				p.PreviousPosition = transform.TransformPoint(p.Position);
			}
		}

		private void Update()
		{
			if (!_awaitingDamaging) return;
            for (int i = 0; i < _damagePoints.Length; i++)
			{
                DamagePoint p = _damagePoints[i];

                var positionWorld = transform.TransformPoint(p.Position);
				var normalWorld = transform.TransformDirection(p.Normal).normalized;

				var vel = (p.PreviousPosition - positionWorld);

				Debug.DrawRay(positionWorld, vel, Color.red, 5);

				p.PreviousPosition = positionWorld;

				if (Physics.Raycast(positionWorld, vel.normalized, out var hit, vel.magnitude))
				{
					if (_ignores.Contains(hit.collider))
					{
						continue;
					}
					if (_hits.Contains(hit.collider)) continue;

					var negativeVel = Vector3.zero;

					if (hit.rigidbody)
					{
						negativeVel = hit.rigidbody.velocity;
					}

					Vector3 effectiveVel = vel;// * effectiveSpeed;
					Vector3 relativeVel = effectiveVel - negativeVel;


					Debug.Log($"Hit! Velocity: {relativeVel}. Speed: {relativeVel.magnitude}, hitTarget = {hit.collider.gameObject}");

					HitReceived?.Invoke(new DamageHitInfo
					{
						Point = hit.point,
						Rigidbody = hit.rigidbody,
						Collider = hit.collider,
						RelativeVelocity = relativeVel,
						Normal = hit.normal
					});

					if (hit.rigidbody)
					{
						var bouncePower = Vector3.Dot(-hit.normal, relativeVel);
						Debug.DrawRay(hit.point, -hit.normal * bouncePower, Color.yellow, 5);
						hit.rigidbody.AddForceAtPosition(-hit.normal * bouncePower, hit.point, ForceMode.Impulse);
					}

					_hits.Add(hit.collider);
					_awaitingDamaging = false;
					return;
				}
			}
		}
	}
}
