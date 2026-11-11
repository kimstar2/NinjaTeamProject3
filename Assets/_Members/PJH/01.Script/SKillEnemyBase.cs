using System;
using _Members.NKY.Scripts;
using UnityEngine;

namespace _Members.PJH._01.Script
{
    public abstract class SKillEnemyBase : AbstractEnemy
    {
        protected SkillEnemy enemySkill;

        protected virtual void Awake()
        {
            enemySkill = GetComponent<SkillEnemy>();
        }
    }
}