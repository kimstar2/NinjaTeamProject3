using UnityEngine;

namespace _Members.KJY._01.Scripts.Agent.Combat.Weapon.Hand
{
    public class WeaponHand : AbstractWeapon
    {
        [field:SerializeField] public AgentHand LeftHand { get; private set; }
        [field:SerializeField] public AgentHand RightHand { get; private set; }
        private bool _rightIsLast;
        
        public override void Init()
        {
            LeftHand.Init();
            RightHand.Init();
        }

        public override void Attack(AttackData data)
        {
            if (_rightIsLast)
                LeftHand.Attack(data.WorldMousePos);
            else
                RightHand.Attack(data.WorldMousePos);
            _rightIsLast = !_rightIsLast;
        }
    }
}