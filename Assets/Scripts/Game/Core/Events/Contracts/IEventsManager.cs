using System;
using System.Collections;
using System.Collections.Generic;

namespace Core.Events.Contracts
{
    public interface IEventsManager
    {
        Dictionary<Type, HandlerBase> Handlers { get; }
        
        void Subscribe<T>(object watcher, Action<T> action) where T : struct;
        
        void Unsubscribe<T>(Action<T> action) where T : struct;

        IEnumerator WaitFor<T>(Predicate<T> filter = null) where T : struct;

        void Fire<T>(T args) where T : struct;

        bool HasWatchers<T>() where T : struct;

        bool CheckSubscribesForObject(object watcher);

        void CheckHandlersOnLoad();

        void CleanUp();
    }
}