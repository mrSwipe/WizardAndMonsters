using System;
using Characters.Contracts;
using Common;
using TMPro;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Characters
{
    public abstract class AbstractEnemy : MonoBehaviour, IEnemy
    {
        [SerializeField] private EnemyMovement movement;
        [SerializeField] private HealthWithProtection healthWithProtection;
        [SerializeField] private Damage damage;
        [SerializeField] private SpawnController spawnController;
        [SerializeField] private TMP_Text heathText;
        [SerializeField] private TMP_Text protectionText;
        [SerializeField] private TMP_Text damageText;
        
        private Player _player;
        private Action<IEnemy> _onTerminateCallback;

        public HealthWithProtection HealthWithProtection => healthWithProtection;
        public float PosY => movement.transform.position.y;
        
        public void Construct(Player player, Vector3 pos, Action<IEnemy> onTerminateCallback)
        {
            _player = player;
            _onTerminateCallback = onTerminateCallback;
            healthWithProtection.OnChangeValue += OnChangeHealthValue;
            healthWithProtection.OnTerminate += OnTerminate;

            transform.position = pos;
            movement.Construct(_player);
            OnChangeHealthValue(HealthWithProtection.HealthValue);

            protectionText.text = $"{HealthWithProtection.ProtectionValue}";
            damageText.text = $"{damage.DamageValue}";
        }

        private void OnChangeHealthValue(int v)
        {
            heathText.text = $"{v}";
        }

        private void OnTerminate()
        {
            Terminate();
            _onTerminateCallback?.Invoke(this);
        }

        public void ResetToStart(int startHealth)
        {
            Terminate();
            HealthWithProtection.ResetHealth(startHealth);
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
                //Debug.Log($">>> Player was damaged {damage.DamageValue} by {gameObject.name}");
                _player.HealthWithProtection.ApplyDamage(damage.DamageValue);
            }
        }
    }
}