using System;
using DG.Tweening;
using FPSMeleeDemo.Core;
using UnityEngine;

namespace FPSMeleeDemo.UI
{
	public struct CursorDamageEvent : IEvent
	{
	}

	public class CursorAnimationHandler
	{
		private const float _damageAnimationDuration = 0.2f;

		private CanvasGroup _lineContainerCanvasGroup;
        private readonly CanvasGroup _blockCanvasGroup;
        private CanvasGroup _deflectCanvasGroup;

        private Sequence _damageTween;
		private Tween _blockTween;
        private Tween _deflectTween;

		private Transform _lineContainer;

		public float LineRadius { get; set; }

		public CursorAnimationHandler(CanvasGroup lineContainerCanvasGroup, CanvasGroup blockCanvasGroup, CanvasGroup deflectCanvasGroup, Transform lineContainer)
		{
			_blockCanvasGroup = blockCanvasGroup;
			_lineContainerCanvasGroup = lineContainerCanvasGroup;
			_deflectCanvasGroup = deflectCanvasGroup;

			_lineContainer = lineContainer;
		}


		public void Cleanup()
		{
			_damageTween?.Kill(true);
		}

		private Tween CreateDamagePositioningTween(bool invert = false)
		{
			const float StartDistance = 8f;
			var tween = DOTween.Sequence();
			for (int i = 0; i < _lineContainer.childCount; i++)
			{
				var t = _lineContainer.GetChild(i);
				
				var fromPos = Cursor.CalculateTransform(i, _lineContainer.childCount, LineRadius + StartDistance);
				var toPos = Cursor.CalculateTransform(i, _lineContainer.childCount, LineRadius);

				if (invert)
					tween.Join(t.DOLocalMove(fromPos.position, _damageAnimationDuration).From(toPos.position));
				else
					tween.Join(t.DOLocalMove(toPos.position, _damageAnimationDuration).From(fromPos.position));
			}

			return tween;
		}
		
		public void PlayDamage()
		{
			_damageTween?.Kill(true);

			_damageTween = DOTween.Sequence();

			_lineContainerCanvasGroup.alpha = 1;
			_damageTween
				.Join(CreateDamagePositioningTween().SetEase(Ease.OutBack))
				.AppendInterval(.4f)
				.Append(_lineContainerCanvasGroup.DOFade(0f, _damageAnimationDuration))
				.Join(CreateDamagePositioningTween(true).SetEase(Ease.InOutSine));
		}


		public void PlayBlock()
		{
			_blockTween?.Kill(true);


			_blockCanvasGroup.alpha = 1;

			_blockTween = _blockCanvasGroup.DOFade(0f, _damageAnimationDuration).SetDelay(.8f);
		}

		public void PlayDeflect()
		{
			_deflectTween?.Kill(true);


			_deflectCanvasGroup.alpha = 1;

			_deflectTween = _deflectCanvasGroup.DOFade(0f, _damageAnimationDuration).SetDelay(.8f);
		}
	}

	public class Cursor : MonoBehaviour
	{
		[SerializeField] private float _radius;
		[SerializeField] private Transform _lineContainer;
		private CanvasGroup _lineContainerCanvasGroup;
		
		private CursorAnimationHandler _animationHandler;
		
		[SerializeField] private CanvasGroup _deflectCanvasGroup;

		[SerializeField] private CursorBlockView _blockView;
		public CursorBlockView BlockView
		{
			get => _blockView;
			set => _blockView = value;
		}

		private void Awake()
		{
			_lineContainerCanvasGroup = _lineContainer.GetComponent<CanvasGroup>();

			_animationHandler = new CursorAnimationHandler(_lineContainerCanvasGroup, _blockView.GetComponent<CanvasGroup>(), _deflectCanvasGroup, _lineContainer);
			_animationHandler.LineRadius = _radius;

			_lineContainerCanvasGroup.alpha = 0;
		}

		private void UpdateLinePositions()
		{
			if (!_lineContainer) return;
			var childCount = _lineContainer.childCount;
			for (int i = 0; i < childCount; i++)
			{
				var line = _lineContainer.GetChild(i);

				var tr = CalculateTransform(i, childCount, _radius);
				line.transform.localEulerAngles = new Vector3(0, 0, tr.angle);
				line.transform.localPosition = tr.position;
			}
		}

		public void ShowDamage()
		{
			_animationHandler.PlayDamage();
		}

		private void OnValidate()
		{
			UpdateLinePositions();
		}

		public static (Vector3 position, float angle) CalculateTransform(int index, int maxIndex, float radius)
		{
			float angle = (360f / maxIndex) * index + (360f / maxIndex * 0.5f);

			float angleRad = Mathf.Deg2Rad * angle;


			var dir = new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));

			return (new Vector3(dir.x * radius, dir.y * radius), angle);
		}

		public void ShowBlock()
		{
			_animationHandler.PlayBlock();
		}

        public void ShowDeflect()
        {
			_animationHandler.PlayDeflect();
        }
    }
}
