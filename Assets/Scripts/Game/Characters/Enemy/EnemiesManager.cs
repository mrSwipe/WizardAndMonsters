using System;
using System.Collections.Generic;
using Characters.Contracts;
using Core;
using Core.Events.Contracts;
using Environment;
using UnityEngine;
using Zenject;
using Random = System.Random;

namespace Characters
{
    internal class EnemiesManager : BaseManager, IEnemiesManager
    {
        private const int MaxEnemies = 10;

        [Inject] private readonly SpawnSettings _spawnSettings;
        [Inject] private readonly IEventsManager _eventsManager;
        
        [Inject] private readonly Player _player;
        [Inject] private readonly EnemyRed.Pool _enemyRedPool;
        [Inject] private readonly EnemyGreen.Pool _enemyGreenPool;
        [Inject] private readonly EnemyBlue.Pool _enemyBluePool;
        
        private readonly Random _rnd = new();
        private List<IEnemy> _enemiesList;

        public bool CanAddEnemy => _enemiesList.Count < MaxEnemies;
        
        public void RecreateEnemy(IEnemy enemy)
        {
            if (_enemiesList.Remove(enemy))
            {
                DespawnEnemy(enemy);
            }
            else
            {
                throw new Exception($"Error! Unknown enemy type {enemy.GetType()}");
            }

            if (CanAddEnemy)
            {
                AddEnemy();
            }
        }
        
        protected override void InitInternal()
        {
            _enemiesList = new List<IEnemy>(15);
            
            for (var i = 0; i < MaxEnemies; i++)
            {
                AddEnemy(); 
            }
        }

        protected override void TerminateInternal()
        {
            ClearEnemiesList();
        }
        
        private void AddEnemy()
        {
            if (!CanAddEnemy) return;
            
            var enemy = CreateRandom();
            var randomPos = _spawnSettings.GetRandomSpawnPosition();
            
            enemy.Construct(_player, new Vector3(randomPos.x, enemy.PosY, randomPos.z));
            _enemiesList.Add(enemy);
        }

        private void DespawnEnemy(IEnemy enemy)
        {
            switch (enemy)
            {
                case EnemyBlue enemyBlue:
                    _enemyBluePool.Despawn(enemyBlue);
                    break;
                case EnemyGreen enemyGreen:
                    _enemyGreenPool.Despawn(enemyGreen);
                    break;
                case EnemyRed enemyRed:
                    _enemyRedPool.Despawn(enemyRed);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(enemy), 
                        $"Error! Unknown enemy type {enemy.GetType()}");
            }
        }
        
        private IEnemy CreateRandom()
        {
            return _rnd.RandomRange(0, 2) switch
            {
                0 => _enemyRedPool.Spawn(),
                1 => _enemyGreenPool.Spawn(),
                2 => _enemyBluePool.Spawn(),
                _ => _enemyRedPool.Spawn()
            };
        }
        
        private void ClearEnemiesList()
        {
            foreach (var enemy in _enemiesList)
            {
                DespawnEnemy(enemy);
            }
            _enemiesList.Clear();
        }
    }
}