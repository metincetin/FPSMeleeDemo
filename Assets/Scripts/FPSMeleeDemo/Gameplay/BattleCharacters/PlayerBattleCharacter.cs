using System;
using FPSMeleeDemo.Core;
using FPSMeleeDemo.Data;
using FPSMeleeDemo.FPS;
using FPSMeleeDemo.Input;
using FPSMeleeDemo.Montage;
using FPSMeleeDemo.Movement;
using FPSMeleeDemo.UI;
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
		private Vector3 _lastLookInput;

		private CardinalDirection _blockingDirection;
		private bool _isBlocking;

		private CharacterLocomotionAnimator _locomotionAnimator;

        public event Action<DamageObject> DamageReceived;

        public void ApplyDamage(DamageObject damage)
		{
			var blockHandler = _attacker.BlockHandler;

			if (blockHandler.IsBlocking)
			{
				float deflectTime = Time.time - blockHandler.BlockStartTime;
				if (deflectTime < 0.3f)
				{

					EventBus<CursorDeflectEvent>.Invoke(new CursorDeflectEvent());
					damage.Damage = 0;
				}
				else
				{
					blockHandler.DepleteDamage(damage);
					EventBus<CursorBlockEvent>.Invoke(new CursorBlockEvent{RemainingBlockPower = blockHandler.RemainingBlockRate});
				}
			}

			if (damage.Damage <= 0) return;
			
			var attr = _attributes;
			attr.Health -= damage.Damage;
			_attributes = attr;
			if (attr.Health <= 0)
			{
				Die();
			}
			DamageReceived?.Invoke(damage);

			Debug.Log($"Player received Damage {_attributes.Health}");
		}

		private void Awake()
		{
			_attributes = new Attributes { Damage = 15, Defence = 2, Health = 50, MaxHealth = 50 };
			_movement = GetComponent<IMovement>();
			_attacker = GetComponent<IAttacker>();

			if (_movement is CharacterMovement characterMovement)
			{
				characterMovement.Manipulators.Add(new RootMotionVelocityManipulator(GetComponentInChildren<RootMotionController>()));
			}

			_locomotionAnimator = new CharacterLocomotionAnimator(GetComponentInChildren<Animator>());
			_attacker.AnimationHandler = new PlayerAttackAnimationHandler(GetComponent<MontagePlayer>());
		}

		private void OnEnable()
		{
			_inputFeeder.Enable();
			_inputFeeder.GameInput.Player.Jump.performed += OnJumpRequested;
			_inputFeeder.GameInput.Player.Dash.performed += OnDashRequested;
			_inputFeeder.GameInput.Player.Attack.performed += OnAttackRequested;

			_inputFeeder.GameInput.Player.Block.performed += OnBlockPerformed;
			_inputFeeder.GameInput.Player.Block.canceled += OnBlockCanceled;

			_inputFeeder.SetCursorState(false);
		}

		private void OnDisable()
		{
			_inputFeeder.Disable();
			_inputFeeder.GameInput.Player.Jump.performed -= OnJumpRequested;
			_inputFeeder.GameInput.Player.Dash.performed -= OnDashRequested;
			_inputFeeder.GameInput.Player.Attack.performed -= OnAttackRequested;

			_inputFeeder.GameInput.Player.Block.performed -= OnBlockPerformed;
			_inputFeeder.GameInput.Player.Block.canceled -= OnBlockCanceled;

			_inputFeeder.SetCursorState(true);
		}


		private void OnBlockPerformed(InputAction.CallbackContext context)
		{
			_attacker.SetBlockState(true);
		}

		private void OnBlockCanceled(InputAction.CallbackContext context)
		{
			_attacker.SetBlockState(false);
		}

		private void OnAttackRequested(InputAction.CallbackContext context)
		{
			_attacker.Attack(_lastLookInput);
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
			var look = _inputFeeder.GameInput.Player.Look.ReadValue<Vector2>();
			if (look.magnitude > 0)
			{
				_lastLookInput = look;
			}
			Move();
			_locomotionAnimator.MovementInput = _movement.MovementInput;
			_locomotionAnimator.Update();
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
