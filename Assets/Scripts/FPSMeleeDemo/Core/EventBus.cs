using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FPSMeleeDemo.Core
{
	public interface IEvent
	{

	}

	public class EventBus<T> where T : IEvent
	{
		public class EventHandle
		{
			public System.Action<T> Event;

			public EventHandle(System.Action<T> ev)
			{
				Event = ev;
			}
		}

		private static HashSet<EventHandle> _events = new();

		[InitializeOnLoadMethod]
		private static void Reset()
		{
			_events = new HashSet<EventHandle>();
		}

		public static EventHandle Register(System.Action<T> ev)
		{
			var handle = new EventHandle(ev);

			_events.Add(handle);

			return handle;
		}

		public static void Unregister(EventHandle handle)
		{
			_events.Remove(handle);
		}

		public static void Invoke(T args)
		{
			foreach (var handle in _events)
			{
				handle.Event(args);
			}
		}

        internal static void Register(object onCursorDamage)
        {
            throw new NotImplementedException();
        }
    }
}
