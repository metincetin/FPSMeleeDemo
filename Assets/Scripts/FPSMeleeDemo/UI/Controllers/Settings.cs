using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace FPSMeleeDemo.UI.Tests
{
	public class Settings : MonoBehaviour
	{
		[SerializeField]
		private CanvasGroup _canvasGroup;

		[SerializeField]
		private CanvasGroup _contentCanvasGroup;

		private Canvas _canvas;
		private GraphicRaycaster _raycaster;

		private Tween _toggleTween;

		public bool IsOpen => _canvas.enabled;

		private void Awake()
		{
			_raycaster = GetComponent<GraphicRaycaster>();
			_canvas = GetComponent<Canvas>();

			_contentCanvasGroup.alpha = 0;
		}

		private Tween CreateToggleTween()
		{
			var seq = DOTween.Sequence();

			seq.Append(
				_canvasGroup.DOFade(1, .2f).From(0)
			);
			seq.Join(_contentCanvasGroup.DOFade(1, .2f).From(0).SetDelay(.1f));
			seq.Join(_contentCanvasGroup.transform.DOLocalMove(Vector3.zero, .2f).From(new Vector3(-50, 50)));

			_toggleTween = seq;
			return seq;
		}

		public void Open()
		{
			_toggleTween?.Kill();

			_canvas.enabled = true;
			_raycaster.enabled = true;

			CreateToggleTween();

		}

		public void Close()
		{
			_toggleTween?.Kill();

			var seq = DOTween.Sequence();

			seq.Append(
				_canvasGroup.DOFade(0, .2f)
			);
			seq.Join(_contentCanvasGroup.DOFade(0, .2f).SetDelay(.1f));
			seq.Join(_contentCanvasGroup.transform.DOLocalMove(new Vector3(-50, 50), .2f));

			seq.AppendCallback(DeactivateCanvas);

			_toggleTween = seq;
		}

		private void DeactivateCanvas()
		{
			_canvas.enabled = false;
			_raycaster.enabled = false;
		}
	}
}

