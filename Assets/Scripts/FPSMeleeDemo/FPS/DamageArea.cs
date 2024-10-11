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
		}

		[System.Serializable]
		private class DamagePoint
		{
			public Vector3 Position;
			public Vector3 Normal;
		}

		[SerializeField]
		private DamagePoint[] _damagePoints;

		private Rigidbody _rigidbody;
		private bool _awaitingDamaging;

		private HashSet<Collider> _ignores = new(5);

		public event Action<DamageHitInfo> HitReceived;

		private void Awake()
		{
			_rigidbody = GetComponent<Rigidbody>();
		}

		public void Begin()
		{
			_awaitingDamaging = true;
		}

		public void End()
		{
			_awaitingDamaging = false;
		}

		private void OnDrawGizmos()
		{
			foreach (var p in _damagePoints)
			{
				var position = transform.TransformPoint(p.Position);
				Gizmos.color = Color.red;
				Gizmos.DrawCube(position, Vector3.one * 0.02f);
				Debug.DrawRay(position, transform.TransformDirection(p.Normal.normalized), Color.red);
				Gizmos.color = Color.white;
			}
		}

		public void AddIgnore(Collider target)
		{
			_ignores.Add(target);
		}

		private void FixedUpdate()
		{
			if (!_awaitingDamaging) return;
			var vel = _rigidbody.velocity;
			foreach (var p in _damagePoints)
			{
				var positionWorld = transform.TransformPoint(p.Position);
				var normalWorld = transform.TransformDirection(p.Normal);

				float effectiveSpeed = Vector3.Dot(vel, normalWorld) * Time.deltaTime;

				if (Physics.Raycast(positionWorld, normalWorld, out var hit, effectiveSpeed))
				{
					if (_ignores.Contains(hit.collider))
					{
						continue;
					}

					var negativeVel = Vector3.zero;

					if (hit.rigidbody)
					{
						negativeVel = hit.rigidbody.velocity;
					}

					Vector3 effectiveVel = vel * effectiveSpeed;
					Vector3 relativeVel = effectiveVel - negativeVel;


					Debug.Log($"Hit! Velocity: {relativeVel}. Speed: {relativeVel.magnitude}, hitTarget = {hit.collider.gameObject}");

					HitReceived?.Invoke(new DamageHitInfo
					{
						Point = hit.point,
						Rigidbody = hit.rigidbody,
						Collider = hit.collider,
						RelativeVelocity = relativeVel
					});

					if (hit.rigidbody)
					{
						hit.rigidbody.AddForceAtPosition(relativeVel * 20, hit.point, ForceMode.Impulse);
					}
					return;
				}
			}
		}


	}
}
