using System;
using _Members.KJY._01.Scripts.Agent.Combat;
using _TevLib.ModuleSystem;
using UnityEngine;

namespace _Members.KJY._01.Scripts.Agent
{
    public abstract class AbstractAgent : ModuleOwner
    {
        public HealthCompo HealthCompo { get; private set; }
        public Hitbox HitBox { get; private set; }

        protected override void InitializeModules()
        {
            base.InitializeModules();
            HealthCompo = GetModule<HealthCompo>();
            HitBox = GetModule<Hitbox>();
        }

        protected override void AfterInitializeModules()
        {
            base.AfterInitializeModules();
            HitBox.OnDamage += HandleDamageApplied;
            HealthCompo.OnDead += HandleDead;
        }

        private void OnDestroy()
        {
            HitBox.OnDamage -= HandleDamageApplied;
            HealthCompo.OnDead -= HandleDead;
        }

        protected abstract void HandleDead();
        
        protected virtual void HandleDamageApplied(DamageData dmgData)
        {
            HealthCompo.TakeDamage(dmgData.Damage);
        }
    }
}