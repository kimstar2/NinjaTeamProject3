using System;
using _Members.KJY._01.Scripts.Player;
using _TevLib.ModuleSystem;
using UnityEngine;

namespace _Members.KJY._01.Scripts.Agent.Combat.Weapon
{
    public abstract class AbstractWeapon : MonoBehaviour , IWeapon
    {
        [field:SerializeField] public SpriteRenderer WeaponRenderer { get; set; }
        public ModuleOwner WeaponOwner { get; private set; }
        public bool IsAttacking { get; protected set; }
        public bool CanChange { get; protected set; } = true;

        public virtual void Init() { }
        public virtual void SetOwner(ModuleOwner moduleOwner)
        {
            WeaponOwner = moduleOwner;
        }
        public abstract void Attack(AttackData data);
    }
}