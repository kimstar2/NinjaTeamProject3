using UnityEngine;

namespace _Members.PJH._01.Script
{
    public abstract class EnemySkill : MonoBehaviour
    {
        protected SkillDataSO skillData;


        public abstract void UseSkill();
        
    }
}
