using System;

namespace Core
{
	public abstract class BaseManager
	{
		public bool IsInitialized { get; private set; }
		public event Action OnInitCompleted;
		public event Action OnTerminationCompleted;

		public void Init()
		{
			if(IsInitialized)
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
