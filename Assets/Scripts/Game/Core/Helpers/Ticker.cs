using System;
using UnityEngine;
using UnityEngine.Profiling;
using Zenject;

namespace Core.Helpers
{
    public class Ticker : ITickable
    {
        public event Action OnTick = () => { };
        public event Action OnOneSecondTickUnscaled = () => { };

        private float nextSecondTick = Time.time;

        public void Tick()
        {
            Profiler.BeginSample("Ticker.Tick");
            OnTick.Invoke();
            Profiler.EndSample();

            var time = Time.unscaledTime;
            if (time < nextSecondTick) return;

            do
            {
                nextSecondTick += 1f;
            } while (nextSecondTick < time);

            Profiler.BeginSample("Ticker.OneSecondTick");
            OnOneSecondTickUnscaled?.Invoke();
            Profiler.EndSample();
        }
    }
}