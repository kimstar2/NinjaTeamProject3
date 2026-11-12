using UnityEngine;

namespace _Members.KJY._01.Scripts.Agent.Combat
{
    public readonly struct DamageData
    {
        public readonly float Damage;
        public readonly Vector2 Direction;

        public DamageData(float damage, Vector2 direction)
        {
            Damage = damage;
            Direction = direction;
        }
    }
}