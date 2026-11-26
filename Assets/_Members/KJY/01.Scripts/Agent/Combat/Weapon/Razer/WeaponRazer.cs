using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Members.KJY._01.Scripts.Agent.Combat.Weapon.Razer
{
    public class WeaponRazer : AbstractWeapon
    {
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
            AsyncCharging().Forget();
        }

        private async UniTaskVoid AsyncCharging()
        {
            // await UniTask.;
        }

        private void StopCharging()
        {
        }
    }
}