using System;
using UnityEngine;

namespace Common
{
    public class HealthWithProtection : MonoBehaviour
    {
        [SerializeField] private int healthValue;
        [Range(0, 1)][SerializeField] private float protectionValue;

        public event Action<int> OnChangeValue;
        public event Action OnTerminate;
        
        public int HealthValue => healthValue;
        public int StartHealthValue { get; private set; }

        public float ProtectionValue => protectionValue;

        public void ApplyDamage(int delta)
        {
            if (healthValue <= 0) return;

            healthValue = healthValue.CalcHealthOnDamage(protectionValue, delta);

            OnChangeValue?.Invoke(healthValue);
            
            if (healthValue <= 0)
            {
                OnTerminate?.Invoke();
            }
        }
        
        public void ApplyHeal(int delta)
        {
            healthValue = healthValue.CalcHealthOnHeal(delta);
        }

        public void ResetHealth(int startHealth)
        {
            SetHealth(startHealth);
        }

        private void Awake()
        {
            StartHealthValue = healthValue;
        }
        
        private void SetHealth(int healthIn)
        {
            healthValue = healthIn;
        }

        private void SetProtection(float protection)
        {
            if (protection is < 0f or > 1f)
            {
                throw new ArgumentException("Error. The min value is greater than the max value");
            }
            protectionValue = protection;
        }
    }
}