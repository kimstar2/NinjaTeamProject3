using _TevLib.HashDataSystem;
using _TevLib.ServiceLocatorSystem.PoolService;
using UnityEngine;

namespace _Members.PJH._01.Script
{
    [CreateAssetMenu(fileName = "SkillDataSO", menuName = "PJH/SO/SkillDataSO")]
    public class SkillDataSO : ScriptableObject
    {
        public int skillID;
        public string skillName;

        public float skillDamage;
        public float coolTime;
        public float castTime;
        public PoolItemSO skillItem;

        public AnimHashSO skillAnimHash;
    }
}
