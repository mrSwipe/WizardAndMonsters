using Common;
using UnityEngine;
using Zenject;

namespace Characters
{
    [RequireComponent (typeof(EnemyMovement))]
    [RequireComponent (typeof(HealthWithProtection))]
    [RequireComponent (typeof(Damage))]
    [RequireComponent (typeof(SpawnController))]
    public class EnemyRed : AbstractEnemy
    {
        public class Pool : MonoMemoryPool<EnemyRed>
        {
            protected override void Reinitialize(EnemyRed enemy)
            {
                enemy.ResetToStart(enemy.StartHealth);
                base.Reinitialize(enemy);
            }
        }
    }
}