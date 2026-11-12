using UnityEngine;

namespace _Members.KJY._01.Scripts.Agent.Combat.Weapon
{
    public class NullWeapon : AbstractWeapon
    {
        public override void Init() {}
        public override void Attack(AttackData data) { }
    }
}