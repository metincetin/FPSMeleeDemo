using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace FPSMeleeDemo.Movement
{

	public class OffsetData
	{
		public Vector3 Value;
		public float FadeDuration;
		public Ease Ease;
	}

	public interface IVelocityOffsetContainer
	{
		public VelocityOffset VelocityOffset { get; }
	}

	public class VelocityOffset
	{
		private List<OffsetData> _offsets = new();

		public void AddOffsetData(OffsetData data)
		{
			_offsets.Add(data);
			DOTween.To(() => data.Value, x => data.Value = x, Vector3.zero, data.FadeDuration)
				.From(data.Value)
				.SetEase(data.Ease)
				.SetTarget(this)
				.OnComplete(() => RemoveData(data));
		}

		private void RemoveData(OffsetData data)
		{
			_offsets.Remove(data);
		}

		public Vector3 Get()
		{
			Vector3 value = Vector3.zero;
			foreach (var offset in _offsets)
			{
				value += offset.Value;
			}

			return value;
		}
	}

	public interface IDash
	{
		public void Dash(Vector3 direction);
	}

	public class CharacterMovement : MonoBehaviour, IMovement, IDash, IVelocityOffsetContainer
	{

		private float _dashTime = 0;
		public const float DashCooldown = 0.6f;

		public bool IsGrounded => _characterController.isGrounded;

		public VelocityOffset VelocityOffset { get; } = new VelocityOffset();

		[SerializeField]
		private bool _alignToOrientation;

		private Vector2 _movementInput;
		public Vector2 MovementInput
		{
			get => _movementInput;
			set
			{
				_movementInput = Vector2.ClampMagnitude(value, 1f);
			}
		}

		public List<IMovementVelocityManipulator> Manipulators = new();

		public Vector3 Velocity => _characterController.velocity;

		[SerializeField]
		private CharacterController _characterController;

		private float _verticalVelocity;

		[SerializeField]
		private float _movementSpeed;

		[SerializeField]
		private float _jumpPower;

		private bool _jumpRequested;

		[SerializeField]
        private float _dashPower = 60;

        private void Update()
		{
			Move();
		}

		private void Move()
		{
			Vector3 velocity = Vector3.zero;

			if (IsGrounded)
			{
				_verticalVelocity = Physics.gravity.y * Time.deltaTime;
			}
			else
			{
				_verticalVelocity += Physics.gravity.y * Time.deltaTime;
			}

			if (_jumpRequested)
			{
				_jumpRequested = false;
				_verticalVelocity = _jumpPower;
			}


			var transformedInputVector = new Vector3(_movementInput.x, 0, _movementInput.y);

			if (_alignToOrientation)
			{
				transformedInputVector = Quaternion.Euler(0, transform.eulerAngles.y, 0) * transformedInputVector;
			}

			var inputVelocity = _movementSpeed * transformedInputVector;

			velocity = (inputVelocity + (Vector3.up * _verticalVelocity) + GetTotalManipulatorVelocity()) * Time.deltaTime;

			velocity += VelocityOffset.Get() * Time.deltaTime;

			_characterController.Move(velocity);
		}

		private Vector3 GetTotalManipulatorVelocity()
		{
			Vector3 result = Vector3.zero;
			foreach (var m in Manipulators)
			{
				result += m.GetVelocity();
			}
			return result;
		}

		public void Jump()
		{
			if (!IsGrounded && !_jumpRequested) return;
			_jumpRequested = true;
		}

		public void Dash(Vector3 direction)
		{
			if (Time.time - _dashTime > DashCooldown)
			{
				VelocityOffset.AddOffsetData(new OffsetData
				{
					Value = direction * _dashPower,
					FadeDuration = 0.3f,
					Ease = Ease.OutExpo
				});
				_dashTime = Time.time;
			}
		}
	}
}
