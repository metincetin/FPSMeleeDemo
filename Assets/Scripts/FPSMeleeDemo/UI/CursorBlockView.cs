using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace FPSMeleeDemo.UI
{
    public class CursorBlockView : MonoBehaviour
    {
        [SerializeField]
        private Image _fill;

		private Tween _fillTween;
        
        public void SetRemainingBlockRate(float perc)
        {
			_fillTween?.Kill();
			if (perc > _fill.fillAmount) 
			{
				_fill.fillAmount = perc;
				return;
			}
            _fillTween = _fill.DOFillAmount(perc, .2f).SetEase(Ease.InOutSine);
        }
    }
}
