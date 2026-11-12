using System;
using UnityEngine;

namespace _Members.PJH._01.Script.Skill
{
    public abstract class AbstractSkill : MonoBehaviour
    {
        protected SkillDataSO SkillData { get; private set; }
        
        public virtual void Init(SkillDataSO skillData)
        {
            SkillData = skillData;
        } 
        
        public abstract void UseSkill();
    }
}