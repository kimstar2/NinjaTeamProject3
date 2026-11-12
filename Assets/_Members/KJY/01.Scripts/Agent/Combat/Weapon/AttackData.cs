using UnityEngine;

namespace _Members.KJY._01.Scripts.Agent.Weapon
{
    public readonly struct AttackData
    {
        public readonly Vector3 WorldMousePos;
        public readonly bool IsAttacking; 

        public AttackData(Vector3 worldMousePos , bool isAttacking)
        {
            WorldMousePos = worldMousePos;
            IsAttacking = isAttacking;
        }
    }
}