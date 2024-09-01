using System;
using Characters.Contracts;
using Common;
using TMPro;
using UnityEngine;
using Zenject;
using Vector3 = UnityEngine.Vector3;

namespace Characters
{
    public abstract class AbstractEnemy : MonoBehaviour, IEnemy, IDamagable
    {
        [SerializeField] private EnemyMovement movement;
        [SerializeField] private HealthWithProtection healthWithProtection;
        [SerializeField] private Damage damage;
        [SerializeField] private SpawnController spawnController;
        [SerializeField] private TMP_Text heathText;
        [SerializeField] private TMP_Text protectionText;
        [SerializeField] private TMP_Text damageText;
        
        [Inject] private IEnemiesManager _enemiesManager;
        
        private ITarget _player;
        
        public int StartHealth => healthWithProtection.StartHealthValue;
        
        public float PosY => movement.transform.position.y;

        public void Construct(ITarget player, Vector3 pos)
        {
            _player = player;
            healthWithProtection.OnChangeValue += OnChangeHealthValue;
            healthWithProtection.OnTerminate += OnTerminate;

            transform.position = pos;
            movement.Construct(_player);
            OnChangeHealthValue(healthWithProtection.HealthValue);

            protectionText.text = $"{healthWithProtection.ProtectionValue}";
            damageText.text = $"{damage.DamageValue}";
        }

        public void ApplyDamage(int damageValue)
        {
            healthWithProtection.ApplyDamage(damageValue);
        }
        
        private void OnChangeHealthValue(int v)
        {
            heathText.text = $"{v}";
        }

        private void OnTerminate()
        {
            Terminate();
            _enemiesManager.RecreateEnemy(this);
        }

        protected void ResetToStart(int startHealth)
        {
            Terminate();
            healthWithProtection.ResetHealth(startHealth);
        }

        private void Terminate()
        {
            movement.Terminate();
            spawnController.SetKinematicAndTriggers(true);
            healthWithProtection.OnTerminate -= OnTerminate;
        }
        
        private void OnCollisionStay(Collision other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                var player = other.gameObject.GetComponent<IDamagable>() 
                            ?? other.gameObject.GetComponentInParent<IDamagable>();

                if (player != null)
                {
                    player.ApplyDamage(damage.DamageValue);
                }
                else
                {
                    throw new Exception($"Error! Can't get Player component from {other.gameObject.name}");
                }
            }
        }
    }
}