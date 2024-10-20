using System;
using System.Linq;
using FPSMeleeDemo.PhysicsModule;
using UnityEngine;

namespace FPSMeleeDemo.Rigging
{

    public class AnimatedRigApplier : MonoBehaviour
	{
		[SerializeField]
		private Transform _physicsRig;

		[SerializeField]
		private Transform _animatedRig;

		private Transform[] _animatedParts;
		private Transform[] _physicalParts;

		private Vector3[] _animationPositions;
		private Quaternion[] _animationRotations;

		[SerializeField]
		private float _distanceError;

		[SerializeField]
		private float _angleError;

		[SerializeField]
		private AnimationUpdateObservable _animationUpdateObservable;

		private void Awake()
		{
			var followers = _physicsRig.GetComponentsInChildren<PhysicsFollower>();

			_physicalParts = followers.Select(x => x.transform).ToArray();

			var animatedParts = _animatedRig.GetComponentsInChildren<Transform>();

			_animatedParts = new Transform[_physicalParts.Length];

			_animationPositions = new Vector3[_physicalParts.Length];
			_animationRotations = new Quaternion[_physicalParts.Length];


			int j = 0;
			foreach (var f in followers)
			{
				for (int i = 0; i < animatedParts.Length; i++)
				{
					Transform animated = animatedParts[i];
					if (animated.gameObject.name == f.gameObject.name)
					{
						_animatedParts[j] = animated.transform;
						j++;
					}
				}
			}
		}

		private void OnEnable()
		{
			_animationUpdateObservable.UpdateReceived += OnAnimatorUpdateReceived;
		}

		private void OnDisable()
		{
			_animationUpdateObservable.UpdateReceived -= OnAnimatorUpdateReceived;
		}

        private void OnAnimatorUpdateReceived()
        {
			UpdatePositions();
        }

        private void UpdatePositions()
		{
            for (int i = 0; i < _animationPositions.Length; i++)
			{
                _animationPositions[i] = _animatedParts[i].position;
                _animationRotations[i] = _animatedParts[i].rotation;
            }
		}

		public void GetAnimationTransform(Transform forPhysicalPart, out Vector3 position, out Quaternion rotation)
		{
			var index = Array.IndexOf(_physicalParts, forPhysicalPart);

			position = forPhysicalPart.position;
			rotation = forPhysicalPart.rotation;

			if (index == 0) return;

			position = _animationPositions[index];
			rotation = _animationRotations[index];
		}

		private void LateUpdate()
		{
			for (int i = 0; i < _animatedParts.Length; i++)
			{
				var animated = _animatedParts[i];
				var physical = _physicalParts[i];

				var animatedPosition = _animationPositions[i];
				var animatedRotation = _animationRotations[i];

				if (Vector3.Distance(animatedPosition, physical.position) > _distanceError)
				{
					animated.position = physical.position;
				}
				if (Quaternion.Angle(animatedRotation, physical.rotation) > _angleError)
				{
					animated.rotation = physical.rotation;
				}
			}
		}
	}
}
