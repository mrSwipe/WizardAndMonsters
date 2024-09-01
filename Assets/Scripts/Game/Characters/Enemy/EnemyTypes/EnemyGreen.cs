using Common;
using UnityEngine;
using Zenject;

namespace Characters
{
    [RequireComponent (typeof(EnemyMovement))]
    [RequireComponent (typeof(HealthWithProtection))]
    [RequireComponent (typeof(Damage))]
    [RequireComponent (typeof(SpawnController))]
    public class EnemyGreen : AbstractEnemy
    {
        public class Pool : MonoMemoryPool<EnemyGreen>
        {
            protected override void Reinitialize(EnemyGreen enemy)
            {
                enemy.ResetToStart(enemy.StartHealth);
                base.Reinitialize(enemy);
            }
        }
    }
}