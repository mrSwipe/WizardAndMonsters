using System;
using Zenject;

namespace Core
{
	public abstract class BaseManager : IInitializable
	{
		public bool IsInitialized { get; private set; }
		public event Action OnInitCompleted;
		public event Action OnTerminationCompleted;

		public void Initialize()
		{
			if (IsInitialized)
			{
				throw new Exception($"Manager {GetType()} has already been initialized");
			}
			
			InitInternal();
			
			IsInitialized = true;
			OnInitCompleted?.Invoke();
		}
		
		public void Terminate()
		{
			if(!IsInitialized)
			{
				throw new Exception($"Manager {GetType()} already terminated");
			}
			TerminateInternal();
			IsInitialized = false;
			OnTerminationCompleted?.Invoke();
		}

		protected abstract void InitInternal();
		protected abstract void TerminateInternal();
	}
}
