using System;
using _Members.NKY.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Members.PJH._01.Script
{
    public class TestEnemy : AbstractEnemy
    {
        SkillModule _skillModule;
        protected override void InitializeModules()
        {
            base.InitializeModules();
            _skillModule = GetModule<SkillModule>();
        }

        protected override void HandleDead()
        {
            Debug.Log("죽음");
        }

        private void Update()
        {
            if (Keyboard.current.yKey.wasPressedThisFrame)
            {
                _skillModule.Skill();
            }
        }
    }
}