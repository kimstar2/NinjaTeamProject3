using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Members.PJH._01.Script
{
    public class TestEnemy : SKillEnemyBase
    {
        private void Update()
        {
            if (Keyboard.current.tKey.wasPressedThisFrame)
            {
                UseSkill();
            }
        }

        public void UseSkill()
        {
            Debug.Log(enemySkill.SkillDataSO.skillID);
        }
    }
}