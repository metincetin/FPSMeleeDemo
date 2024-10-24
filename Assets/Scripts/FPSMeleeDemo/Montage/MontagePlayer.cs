using System;
using UnityEngine;
using UnityEngine.Playables;
using Action = System.Action;

namespace FPSMeleeDemo.Montage
{
	[RequireComponent(typeof(PlayableDirector))]
	[ExecuteAlways]
	public partial class MontagePlayer : MonoBehaviour, INotificationReceiver
	{
		private MontageHandle _currentMontageHandle;
		public PlayableDirector PlayableDirector { get; private set; }

		private Action<MontageEvent> _queuedCalls;

		private void Awake()
		{
			PlayableDirector = GetComponent<PlayableDirector>();
		}

		private void OnEnable()
		{
			PlayableDirector.stopped += OnDirectorStopped;
		}

		private void OnDisable()
		{
			PlayableDirector.stopped -= OnDirectorStopped;
		}

		public void OnNotify(Playable origin, INotification notification, object context)
		{
			if (notification is MontageMarker marker) marker.Notify(gameObject, this);
		}

		public MontageHandle PlayMontage(PlayableAsset asset)
		{
			_currentMontageHandle?.InvokeEnded();
			PlayableDirector.Play(asset);
			_currentMontageHandle = new MontageHandle { Montage = asset };
			return _currentMontageHandle;
		}

		private void OnDirectorStopped(PlayableDirector obj)
		{
			_currentMontageHandle?.InvokeEnded();
			_currentMontageHandle = null;
			_queuedCalls = null;
		}

		public MontageHandle PlayMontageAndWaitEvent(PlayableAsset asset, Action<MontageEvent> ev)
		{
			_queuedCalls = null;
			_currentMontageHandle?.InvokeEnded();
			_queuedCalls += ev;
			return PlayMontage(asset);
		}

		public void InvokeEvent(MontageMarker sender, object payload)
		{
			_queuedCalls?.Invoke(new MontageEvent { Sender = sender, Payload = payload });
		}

		public void EndMontage(bool invokeQueuedEvents = false)
		{
			if (invokeQueuedEvents)
			{
				_queuedCalls?.Invoke(new MontageEvent());
				_queuedCalls = null;
			}
			PlayableDirector.Stop();
		}

		public class MontageHandle
		{
			public PlayableAsset Montage;
			public event Action Ended;

			internal void InvokeEnded()
			{
				Ended?.Invoke();
			}
		}

		public struct MontageEvent
		{
			public object Payload;
			public MontageMarker Sender;
		}
	}
}
