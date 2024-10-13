using System;
using FPSMeleeDemo.Data;
using FPSMeleeDemo.FPS;
using FPSMeleeDemo.Input;
using FPSMeleeDemo.Movement;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FPSMeleeDemo.Gameplay.BattleCharacters
{

	public class PlayerBattleCharacter : MonoBehaviour, IBattleCharacter, IDamageReceiver
	{
		private Attributes _attributes;
		public Attributes Attributes => _attributes;

		[SerializeField]
		private InputFeeder _inputFeeder;

		[SerializeField]
		private IMovement _movement;

		[SerializeField]
		private IAttacker _attacker;

		private Vector3 _lastMovement;

		private CardinalDirection _blockingDirection;
		private bool _isBlocking;

		public void ApplyDamage(DamageObject damage)
		{
			var attr = _attributes;
			attr.Health -= damage.Damage;
			_attributes = attr;

			if (attr.Health <= 0)
			{
				Die();
			}
		}

		private void Awake()
		{
			_attributes = new Attributes { Damage = 5, Defence = 2, Health = 50 };
			_movement = GetComponent<IMovement>();
			_attacker = GetComponent<IAttacker>();
		}

		private void OnEnable()
		{
			_inputFeeder.Enable();
			_inputFeeder.GameInput.Player.Jump.performed += OnJumpRequested;
			_inputFeeder.GameInput.Player.Dash.performed += OnDashRequested;
			_inputFeeder.GameInput.Player.Attack.performed += OnAttackRequested;

			_inputFeeder.SetCursor(false);
		}

		private void OnDisable()
		{
			_inputFeeder.Disable();
			_inputFeeder.GameInput.Player.Jump.performed -= OnJumpRequested;
			_inputFeeder.GameInput.Player.Dash.performed -= OnDashRequested;
			_inputFeeder.GameInput.Player.Attack.performed -= OnAttackRequested;

			_inputFeeder.SetCursor(true);
		}

		private void OnAttackRequested(InputAction.CallbackContext context)
		{
			_attacker.Attack(_inputFeeder.GameInput.Player.Look.ReadValue<Vector2>());
		}

		private void OnJumpRequested(InputAction.CallbackContext context)
		{
			_movement.Jump();
		}

		private void OnDashRequested(InputAction.CallbackContext context)
		{
			if (_movement is IDash dash)
			{
				dash.Dash(Quaternion.Euler(0, transform.eulerAngles.y, 0) * new Vector3(_lastMovement.x, 0, _lastMovement.y).normalized);
			}
		}

		public void Die()
		{
			Debug.Log("You died!");
		}

		private void Update()
		{
			Move();
		}

		private void Move()
		{
			var input = _inputFeeder.GameInput.Player.Movement.ReadValue<Vector2>();
			_movement.MovementInput = input;

			if (input.magnitude > 0)
				_lastMovement = input;
		}
	}
}
