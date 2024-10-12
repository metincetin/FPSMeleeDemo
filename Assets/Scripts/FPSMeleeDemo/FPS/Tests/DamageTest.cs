using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FPSMeleeDemo.Input;
using UnityEngine.InputSystem;
using System;

namespace FPSMeleeDemo.FPS.Tests
{
	public class DamageTest : MonoBehaviour
	{
		[SerializeField]
		private InputFeeder _inputFeeder;

		private DamageArea _damageArea;

		private Animator _animator;

		private void Awake()
		{
			_animator = GetComponentInChildren<Animator>();

			_damageArea = GetComponentInChildren<DamageArea>();

			_damageArea.AddIgnore(GetComponent<Collider>());
		}

		private void OnEnable()
		{
			_inputFeeder.Enable();
			_inputFeeder.GameInput.Player.Attack.performed += OnAttackPerformed;
			_damageArea.HitReceived += OnHitReceived;
		}

		private void OnDisable()
		{
			_inputFeeder.Disable();
			_inputFeeder.GameInput.Player.Attack.performed -= OnAttackPerformed;
			_damageArea.HitReceived -= OnHitReceived;
		}

        private void OnHitReceived(DamageArea.DamageHitInfo info)
        {
			StartCoroutine(HandleAnimation());
        }

		private IEnumerator HandleAnimation()
		{
			yield return new WaitForFixedUpdate();
			_animator.speed = 0;
			yield return new WaitForSeconds(0.2f);
			_animator.speed = 1;
			_animator.SetTrigger("ExitAttack");
		}

        private void OnAttackPerformed(InputAction.CallbackContext context)
        {
			_animator.SetTrigger("Attack");

			StopCoroutine(BeginDamage());
			StartCoroutine(BeginDamage());
        }
		private IEnumerator BeginDamage()
		{
			yield return new WaitForSeconds(0.2f);
			_damageArea.Begin();
			yield return new WaitForSeconds(1.8f);
			_damageArea.End();
		}
	}
}
