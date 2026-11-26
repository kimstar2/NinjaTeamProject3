using System;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace _Members.KJY._01.Scripts.Agent.Combat.Weapon.Razer
{
    public class WeaponRazer : AbstractWeapon
    {
        [SerializeField] private float maxChargingTime = 3f;
        [SerializeField] private float minChargingTime = 0.5f;
        
        [Header("Event Setting")]
        [HorizontalLine(color:EColor.Gray)]
        [SerializeField] private UnityEvent onStartCharging;
        [SerializeField] private UnityEvent<float> onFire;
        
        
        private bool _onCharging;

        public override void Attack(AttackData data)
        {
            if (data.IsAttacking)
                OnCharging();
            else
                StopCharging();
        }

        private void OnCharging()
        {
            _onCharging = true;
            onStartCharging?.Invoke();
            
            AsyncCharging().Forget();
        }

        private async UniTaskVoid AsyncCharging()
        {
            float cumulativeTime = 0f;
            while (_onCharging)
            {
                cumulativeTime += Time.deltaTime;
                Debug.Log(cumulativeTime);
                if (cumulativeTime > maxChargingTime)
                    return;
                await UniTask.Yield();
            }
            if (cumulativeTime < minChargingTime)
                return;
            
            Fire(cumulativeTime);
        }

        private void Fire(float cumulativeTime)
        {
            Debug.Log("Fire : " + cumulativeTime);
            onFire?.Invoke(cumulativeTime);
        }

        private void StopCharging()
        {
            _onCharging = false;
        }
    }
}