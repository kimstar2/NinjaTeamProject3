using UnityEngine;

namespace _Members.CJY.Scripts
{
    [CreateAssetMenu(fileName = "BossType", menuName = "CJY/BossType", order = 0)]
    public class BossDataSO : ScriptableObject
    {
        [Header("기본 정보")]
        public string Name;
        public string Desc;
        public LayerMask AttackTarget;
        
        [Header("능력치")]
        public int HP;
        public float Speed;
        
        public float DetectRange;
        public float AttackRange;
        public float AttackPower;

        public float AttackSpeed;
        public float AttackCool;
        public float Attack2Cool;
        public float SPAttackCool;
        public float ChargeCool;
    }
}