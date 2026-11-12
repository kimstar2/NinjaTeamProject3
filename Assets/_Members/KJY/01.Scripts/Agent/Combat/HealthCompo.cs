using System;
using _TevLib.ModuleSystem;
using UnityEngine;

namespace _Members.KJY._01.Scripts.Agent.Combat
{
    public class HealthCompo : MonoModule
    {
        [field: SerializeField] public float MaxHealth { get; private set; }
        
        public event Action<float> OnHealthChanged; 
        public event Action OnDead; 
        
        private float _currentHealth;
        public float CurrentHealth
        {
            get => _currentHealth;
            set
            {
                _currentHealth = Mathf.Clamp(value, 0, MaxHealth);
                OnHealthChanged?.Invoke(_currentHealth);
            }
        }
        public float GetHealthPer => CurrentHealth / MaxHealth; 
        public bool IsDead => CurrentHealth <= 0;
        
        public void TakeDamage(float damage)
        {
            if (IsDead) return;
            CurrentHealth -= damage;
            if (IsDead)
                OnDead?.Invoke();
        }
    }
}