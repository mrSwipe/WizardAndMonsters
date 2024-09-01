using System;
using Characters.Contracts;
using Common;
using Spells.Contracts;
using UnityEngine;

namespace Spells
{
    public abstract class AbstractSpell : MonoBehaviour, ISpell
    {
        [SerializeField] private SpellMovement movement;
        [SerializeField] private Damage damage;

        private float _lifeTimer;
        private Action<ISpell> _onTerminateCallback;
        
        public abstract SpellType SpellType { get; }
        public int Damage => damage.DamageValue;
        
        public void Fire(Transform firePoint, float lifeTimeSec, Action<ISpell> onTerminateCallback)
        {
            _lifeTimer = lifeTimeSec;
            _onTerminateCallback = onTerminateCallback;
            
            movement.Fire(firePoint);
        }

        protected void ResetToStart()
        {
            _lifeTimer = 0;
            movement.Terminate();
        }
        
        private void Update()
        {
            if (!movement.IsMoving) return;
            
            _lifeTimer -= Time.deltaTime;
            
            if (_lifeTimer <= 0)
            {
                _onTerminateCallback?.Invoke(this);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                var enemy = other.gameObject.GetComponent<IEnemy>() 
                            ?? other.gameObject.GetComponentInParent<IEnemy>();

                if (enemy != null)
                {
                    enemy.HealthWithProtection.ApplyDamage(Damage);
                    _onTerminateCallback?.Invoke(this);
                }
                else
                {
                    throw new Exception($"Error! Can't get Enemy component from {other.gameObject.name}");
                }
            }
            else if (!other.gameObject.CompareTag("Player"))
            {
                _onTerminateCallback?.Invoke(this);
            }
        }
    }
}