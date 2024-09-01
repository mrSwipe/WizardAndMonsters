using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Profiling;

namespace Core.Events
{
	public abstract class HandlerBase
	{
		public static bool LogsEnabled => false;

		public static bool AllFireLogs => LogsEnabled;

		public List<object> Watchers { get; protected set; } = new(100);

		public virtual void CleanUp()
		{
		}

		public virtual bool FixWatchers()
		{
			return false;
		}

		protected void EnsureWatchers()
		{
			Watchers ??= new List<object>(100);
		}
	}

	public class Handler<T> : HandlerBase
	{
		private bool safeEnumerationActionsDirty;
		private readonly List<Action<T>> saveEnumerationActions = new(25);
		private readonly List<Action<T>> actions = new(25);
		private readonly List<Action<T>> removed = new(25);
		private bool isFireInProcess;

		public void Subscribe(object watcher, Action<T> action)
		{
			if (removed.Contains(action))
			{
				removed.Remove(action);
			}

			if (!actions.Contains(action))
			{
				safeEnumerationActionsDirty = true;
				actions.Add(action);
				EnsureWatchers();
				Watchers.Add(watcher);
			}
			else if (LogsEnabled)
			{
				Debug.LogFormat("{0} tries to subscribe to {1} again.", watcher, action);
			}
		}

		public void Unsubscribe(Action<T> action)
		{
			SafeUnsubscribe(action);
		}

		public void Fire(T arg)
		{
			isFireInProcess = true;
			if (safeEnumerationActionsDirty)
			{
				Profiler.BeginSample("EventManager.RefreshActionsList");
				saveEnumerationActions.Clear();
				saveEnumerationActions.AddRange(actions);
				safeEnumerationActionsDirty = false;
				Profiler.EndSample();
			}
			foreach (var current in saveEnumerationActions.Where(current => !removed.Contains(current)))
			{
				current.Invoke(arg);
			}

			isFireInProcess = false;
			CleanUp();
			if (AllFireLogs)
			{
				Debug.LogFormat("[{0}] fired (Listeners: {1})", typeof(T).Name, Watchers.Count);
			}
		}

		public override void CleanUp()
		{
			Profiler.BeginSample("EventManager.Handler.CleanUp");
			foreach (var item in removed)
			{
				FullUnsubscribe(item);
			}

			removed.Clear();
			Profiler.EndSample();
		}

		public override bool FixWatchers()
		{
			CleanUp();
			var count = 0;
			EnsureWatchers();
			for (var i = 0; i < Watchers.Count; i++)
			{
				var watcher = Watchers[i];
				if (watcher is not MonoBehaviour comp) continue;
				if (comp) continue;
				SafeUnsubscribe(i);
				count++;
			}

			if (count > 0)
			{
				CleanUp();
				
				if (LogsEnabled)
				{
					Debug.LogFormat("{0} destroyed scripts subscribed to event {1}.", count, typeof(T));
				}
			}
			
			return count == 0;
		}
		
		private void SafeUnsubscribe(Action<T> action)
		{
			var index = actions.IndexOf(action);
			if (isFireInProcess)
			{
				SafeUnsubscribe(index);
			}
			else
			{
				FullUnsubscribe(index);
			}
			
			if (index < 0 && LogsEnabled)
			{
				Debug.LogFormat("Trying to unsubscribe action {0} without watcher.", action);
			}
		}

		private void SafeUnsubscribe(int index)
		{
			if (index >= 0 && index < actions.Count)
			{
				removed.Add(actions[index]);
			}
		}

		private void FullUnsubscribe(int index)
		{
			if (index < 0) return;
			safeEnumerationActionsDirty = true;
			actions.RemoveAt(index);
			if (index < Watchers.Count)
			{
				Watchers.RemoveAt(index);
			}
		}

		private void FullUnsubscribe(Action<T> action)
		{
			var index = actions.IndexOf(action);
			FullUnsubscribe(index);
		}
	}
}