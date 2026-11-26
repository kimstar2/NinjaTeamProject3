using System;
using UnityEngine;

namespace _Members.PJH._01.Script.Skill
{
    public class FireBallSkill : AbstractSkill
    {


        public override void UseSkill()
        {
            Debug.Log("파이어 볼 발사");
            // Instantiate(SkillData.skillItem);
        }
    }
}