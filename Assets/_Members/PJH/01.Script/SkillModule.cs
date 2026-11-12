using System;
using _Members.NKY.Scripts;
using _Members.PJH._01.Script.Skill;
using _TevLib.ModuleSystem;
using UnityEngine;

namespace _Members.PJH._01.Script
{
    public class SkillModule : MonoModule 
    {
        [SerializeField] private AbstractSkill skill;
        [SerializeField] private SkillDataSO skillData;

        public override void Init(ModuleOwner owner)
        {
            base.Init(owner);
            skill.Init(skillData);
        }

        public void Skill()
        {
            skill.UseSkill();
        }

    }
}
