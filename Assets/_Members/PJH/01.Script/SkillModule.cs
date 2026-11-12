using System;
using _Members.NKY.Scripts;
using _Members.PJH._01.Script.Skill;
using _TevLib.ModuleSystem;
using UnityEngine;

namespace _Members.PJH._01.Script
{
    public class SkillModule : MonoModule 
    {
        [SerializeField] private AbstractSkill _skill;

        public override void Init(ModuleOwner owner)
        {
            base.Init(owner);
        }

        public void Skill()
        {
            _skill.UseSkill();
        }

    }
}
