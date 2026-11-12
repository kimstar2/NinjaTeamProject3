using UnityEngine;

namespace _Members.KJY._01.Scripts.Agent.Combat.Weapon
{
    public abstract class AbstractWeapon : MonoBehaviour , IWeapon
    {
        public abstract void Init();
        public abstract void Attack(AttackData data);
    }
}