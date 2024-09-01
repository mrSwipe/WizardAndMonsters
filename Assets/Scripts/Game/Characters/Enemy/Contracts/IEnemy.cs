using System;
using Common;
using UnityEngine;

namespace Characters.Contracts
{
    public interface IEnemy
    {
        float PosY { get; }

        HealthWithProtection HealthWithProtection { get; }
        
        void Construct(Player player, Vector3 pos, Action<IEnemy> onTerminateCallback);
    }
}