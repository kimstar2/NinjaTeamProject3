using System;
using _TevLib.ModuleSystem;
using UnityEngine;
using ZLogger;

namespace _Members.KJY._01.Scripts.Agent.Combat
{
    public class Hitbox : MonoModule , IDamageable
    {
        public Collider2D HitboxCollider {get; private set;}

        public override void Init(ModuleOwner owner)
        {
            base.Init(owner);
            HitboxCollider = GetComponent<Collider2D>();
        }
        
        public event Action<DamageData> OnDamage;
        public void ApplyDamage(DamageData damageData)
        {
            OnDamage?.Invoke(damageData);
        }
    }
}