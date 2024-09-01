using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Core.Events.Contracts;
using Core.Helpers;
using UnityEngine;
using Zenject;

namespace Core.Events
{
	internal class EventsManager : BaseManager, IEventsManager
	{
		private const int CleanUpInterval = 10;
		
		[Inject] private readonly Ticker _ticker;
		
		private int _cleanupTimer;
		
		public Dictionary<Type, HandlerBase> Handlers { get; } = new(100);
		
		public void Subscribe<T>(object watcher, Action<T> action) where T : struct
		{
			SubscribeInternal(watcher, action);
		}

		public void Unsubscribe<T>(Action<T> action) where T : struct
		{
			UnsubscribeInternal(action);
		}

		public IEnumerator WaitFor<T>(Predicate<T> filter = null) where T : struct
		{
			var hasFired = false;
			Action<T> handler = ev =>
			{
				if (filter == null) hasFired = true;
				else if (filter(ev)) hasFired = true;
			};
			Subscribe(handler, handler);
			while (!hasFired) yield return null;
			Unsubscribe(handler);
		}

		public void Fire<T>(T args) where T : struct
		{
			FireEvent(args);
		}

		public bool HasWatchers<T>() where T : struct
		{
			return HasWatchersDirect<T>();
		}
		
		public bool CheckSubscribesForObject(object watcher)
		{
			var isExistSubscribes = false;
			var msg = new StringBuilder();
			foreach (var item in Handlers)
			{
				if (!item.Value.Watchers.Contains(watcher))
				{
					continue;
				}
				isExistSubscribes = true;
				msg.AppendLine($"[EventManager] Object {watcher} exist subscribe on {item.Key}");
			}

			if (isExistSubscribes)
			{
				Debug.Log(msg.ToString());
			}
			
			return isExistSubscribes;
		}
		
		public void CheckHandlersOnLoad()
		{
			foreach (var item in Handlers)
			{
				item.Value.FixWatchers();
			}
		}

		public void CleanUp()
		{
			foreach (var item in Handlers)
			{
				item.Value.CleanUp();
			}
		}
		
		protected override void InitInternal()
		{
			_ticker.OnOneSecondTickUnscaled += OnOneSecondTickUnscaled;
		}

		protected override void TerminateInternal()
		{
			_ticker.OnOneSecondTickUnscaled -= OnOneSecondTickUnscaled;
		}

		private void OnOneSecondTickUnscaled()
		{
			TryCleanUp();
		}

		private void TryCleanUp()
		{
			if (_cleanupTimer > CleanUpInterval)
			{
				CleanUp();
				_cleanupTimer = 0;
			}
			else
			{
				_cleanupTimer += 1;
			}
		}

		private void SubscribeInternal<T>(object watcher, Action<T> action)
		{
			if (!Handlers.TryGetValue(typeof(T), out var handler))
			{
				handler = new Handler<T>();
				Handlers.Add(typeof(T), handler);
			}

			(handler as Handler<T>)?.Subscribe(watcher, action);
		}

		private void UnsubscribeInternal<T>(Action<T> action)
		{
			if (Handlers.TryGetValue(typeof(T), out var handler))
			{
				(handler as Handler<T>)?.Unsubscribe(action);
			}
		}

		private void FireEvent<T>(T args)
		{
			if (!Handlers.TryGetValue(typeof(T), out var handler))
			{
				handler = new Handler<T>();
				Handlers.Add(typeof(T), handler);
			}

			(handler as Handler<T>)?.Fire(args);
		}

		private bool HasWatchersDirect<T>() where T : struct
		{
			if (Handlers.TryGetValue(typeof(T), out var container))
			{
				return container.Watchers.Count > 0;
			}

			return false;
		}
	}
}