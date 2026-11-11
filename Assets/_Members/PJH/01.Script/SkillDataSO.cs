using UnityEngine;

namespace _Members.PJH._01.Script
{
    [CreateAssetMenu(fileName = "SkillSO", menuName = "PJH/SkillData/SkillSO")]
    public class SkillDataSO : ScriptableObject
    {
        public string skillName;
        public float skillDamage;
        public float skillRange;
        public float coolTime;
        public float castTime;
        public string animatorHash;
        public GameObject skillPrefab;
    }
}
