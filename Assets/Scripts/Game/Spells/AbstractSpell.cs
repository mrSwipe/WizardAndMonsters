using System;
using Common;
using Spells.Contracts;
using UnityEngine;
using Zenject;

namespace Spells
{
    public abstract class AbstractSpell : MonoBehaviour, ISpell
    {
        [SerializeField] private SpellMovement movement;
        [SerializeField] private Damage damage;

        [Inject] private readonly ISpellsManager _spellsManager;
        
        private float _lifeTimer;
        
        public abstract SpellType SpellType { get; }
        public int Damage => damage.DamageValue;
        
        public void Fire(Transform firePoint, float lifeTimeSec)
        {
            _lifeTimer = lifeTimeSec;
            
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
                _spellsManager.RemoveSpell(this);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Damagable"))
            {
                var enemy = other.gameObject.GetComponent<IDamagable>() 
                            ?? other.gameObject.GetComponentInParent<IDamagable>();

                if (enemy != null)
                {
                    enemy.ApplyDamage(Damage);
                    _spellsManager.RemoveSpell(this);
                }
                else
                {
                    throw new Exception($"Error! Can't get Enemy component from {other.gameObject.name}");
                }
            }
            else if (!other.gameObject.CompareTag("Player"))
            {
                _spellsManager.RemoveSpell(this);
            }
        }
    }
}