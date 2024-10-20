using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using FPSMeleeDemo.Gameplay;
using UnityEngine;
using UnityEngine.UI;

namespace FPSMeleeDemo.UI
{
	public class Healthbar : MonoBehaviour, IDependant<IBattleCharacter>
	{
		private IBattleCharacter _battleCharacter;
		IBattleCharacter IDependant<IBattleCharacter>.Value
		{
			get => _battleCharacter;
			set
			{

				if (_battleCharacter == value) return;
				if (_battleCharacter != null)
				{
					_battleCharacter.DamageReceived -= OnDamageReceived;
				}
				_battleCharacter = value;
				_battleCharacter.DamageReceived += OnDamageReceived;

				ForceApplyValues();
			}
		}

		[SerializeField]
		private Image _fill;

		[SerializeField]
		private Image _change;
		private Sequence _fillTween;

		private void OnDamageReceived(DamageObject @object)
		{
			ApplyValues();
		}

		private void ForceApplyValues()
		{
			var healthPerc = _battleCharacter.Attributes.Health / _battleCharacter.Attributes.MaxHealth;
			_fill.fillAmount = healthPerc;
			_change.fillAmount = healthPerc;
		}

		private void ApplyValues()
		{
			var healthPerc = _battleCharacter.Attributes.Health / _battleCharacter.Attributes.MaxHealth;

			_fillTween?.Kill();

			_fillTween = DOTween.Sequence();
			_fillTween
				.Append(_fill.DOFillAmount(healthPerc, .2f))
				.AppendInterval(.4f)
				.Append(_change.DOFillAmount(healthPerc, .2f))

				.SetEase(Ease.InOutSine);
		}
	}
}
