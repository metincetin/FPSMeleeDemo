using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using FPSMeleeDemo.Core;
using FPSMeleeDemo.Gameplay.BattleCharacters;
using UnityEngine;
using UnityEngine.UI;

namespace FPSMeleeDemo.FX
{
    public class DamageEffectController : MonoBehaviour
    {
        [SerializeField]
        private Image _image;
        
        private EventBus<PlayerBattleCharacter.PlayerDamageEvent>.EventHandle _handle;

        private Tween _fadeTween;

        private void OnEnable()
        {
            _handle = EventBus<PlayerBattleCharacter.PlayerDamageEvent>.Register(OnPlayerDamageReceived);
        }


        private void OnDisable()
        {
            EventBus<PlayerBattleCharacter.PlayerDamageEvent>.Unregister(_handle);
            this.DOKill(true);
        }
        
        private void OnPlayerDamageReceived(PlayerBattleCharacter.PlayerDamageEvent obj)
        {
            _fadeTween?.Kill(true);
            var seq = DOTween.Sequence();
            seq.Append(_image.DOFade(1f,.1f).From(0));
            seq.AppendInterval(0.2f);
            seq.Append(_image.DOFade(0f,.2f));

            seq.SetTarget(this);

            _fadeTween = seq;
        }
    }
}