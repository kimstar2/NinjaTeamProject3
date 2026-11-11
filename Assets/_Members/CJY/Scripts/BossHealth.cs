using System;
using System.ComponentModel.DataAnnotations;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

namespace _Members.CJY.Scripts
{
    public class BossHealth : MonoBehaviour, IDamageable
    {
        [SerializeField] private TextMeshProUGUI bossHpText;
        
        public int MaxHp { get; private set; }
        public int CurHp { get; private set; } 
        public bool IsDead { get; private set; }
        
        public event Action<int> OnHealthChanged; 
        public event Action<int> OnHit;   
        public event Action OnDead;

        private float _lastHitTime;

        public void Init(int maxHp)
        {
            MaxHp = maxHp;
            CurHp = maxHp;
            IsDead = false;
            _lastHitTime = 0;
            
            OnHealthChanged += TextUpdate;
            OnHealthChanged?.Invoke(CurHp);
        }

        private void TextUpdate(int hp)
        {
            bossHpText.text = $"Boss HP : {hp.ToString()}";
        }

        public void TakeDamage(int damage)
        {
            if (IsDead || damage <= 0) return;
            if (Time.time < _lastHitTime) return;

            _lastHitTime = Time.time;
            CurHp = Mathf.Max(CurHp - damage, 0);

            OnHit?.Invoke(damage);
            OnHealthChanged?.Invoke(CurHp);

            if (CurHp <= 0)
            {
                IsDead = true;
                OnDead?.Invoke();
            }
        }

        public void Heal(int heal)
        {
            if (IsDead || heal <= 0) return;

            CurHp = Mathf.Min(CurHp + heal, MaxHp);
            OnHealthChanged?.Invoke(CurHp);
        }

        private void OnDisable()
        {
            OnHealthChanged -= TextUpdate;
        }
    }
}