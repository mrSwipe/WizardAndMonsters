using Common;
using UnityEngine;
using Zenject;

namespace Characters
{
    [RequireComponent (typeof(EnemyMovement))]
    [RequireComponent (typeof(HealthWithProtection))]
    [RequireComponent (typeof(Damage))]
    [RequireComponent (typeof(SpawnController))]
    public class EnemyBlue : AbstractEnemy
    {
        public class Pool : MonoMemoryPool<EnemyBlue>
        {
            protected override void Reinitialize(EnemyBlue enemy)
            {
                enemy.ResetToStart(enemy.StartHealth);
                base.Reinitialize(enemy);
            }
        }
    }
}