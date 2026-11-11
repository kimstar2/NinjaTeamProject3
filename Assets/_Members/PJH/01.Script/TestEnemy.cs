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

        private void Update()
        {
            if (Keyboard.current.yKey.wasPressedThisFrame)
            {
                _skillModule.Skill();
            }
        }
    }
}