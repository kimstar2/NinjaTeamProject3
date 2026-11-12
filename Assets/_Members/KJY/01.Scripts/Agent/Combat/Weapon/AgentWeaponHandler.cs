using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using _Members.KJY._01.Scripts.Agent.Combat.Weapon.Dagger;
using _Members.KJY._01.Scripts.Object.Rotate;
using _TevLib.ModuleSystem;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Members.KJY._01.Scripts.Agent.Combat.Weapon
{
    public class AgentWeaponHandler : RotateObject , IModule
    {
        [SerializeField] private WeaponType firstWeapon;
        [SerializeField] private List<WeaponDataSO> defaultWeapons;
        private readonly Dictionary<WeaponType, AbstractWeapon> _weapons = new();
        public AbstractWeapon CurrentWeapon { get; private set; }

        private void Awake()
        {
            GameObject nullWeapon = new GameObject();
            CurrentWeapon = nullWeapon.AddComponent<NullWeapon>();
        }

        public void Init()
        {
            foreach (WeaponDataSO defaultWeapon in defaultWeapons)
                AddWeapon(defaultWeapon.WeaponType,defaultWeapon.Weapon);
            ChangeWeapon(firstWeapon);
        }

        public void AddWeapon(WeaponType weaponType,AbstractWeapon weapon)
        {
            AbstractWeapon weaponGo = Instantiate(weapon,transform);
            weaponGo.gameObject.SetActive(false);
            _weapons.Add(weaponType,weaponGo);
        }

        public async void ChangeWeapon(WeaponType weaponType)
        {
            try
            {
                await UniTask.WaitUntil(() => CurrentWeapon is {CanChange: true});
            
                if (_weapons.TryGetValue(weaponType, out AbstractWeapon weapon))
                {
                    CurrentWeapon.gameObject.SetActive(false);
                    CurrentWeapon = weapon;
                    CurrentWeapon.Init();
                    CurrentWeapon.gameObject.SetActive(true);
                }
            }
            catch (OperationCanceledException) { }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public void Attack(AttackData attackData)
        {
            if (CurrentWeapon is {IsAttacking: true}) return;
            if (!attackData.IsAttacking) return;
            CurrentWeapon?.Attack(attackData);
        }
    }
}