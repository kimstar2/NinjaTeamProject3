using System;
using UnityEngine;

namespace _Members.KJY._01.Scripts.Agent.Combat.Weapon
{
    public abstract class AbstractWeapon : MonoBehaviour , IWeapon
    {
        public bool IsAttacking { get; protected set; }
        public bool CanChange { get; protected set; } = true;
        public abstract void Init();
        public abstract void Attack(AttackData data);
    }
}