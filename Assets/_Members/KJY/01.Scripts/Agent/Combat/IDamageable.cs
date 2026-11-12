using System;

namespace _Members.KJY._01.Scripts.Agent.Combat
{
    public interface IDamageable
    {
        event Action<DamageData> OnDamage;
        void ApplyDamage(DamageData damageData);
    }
}