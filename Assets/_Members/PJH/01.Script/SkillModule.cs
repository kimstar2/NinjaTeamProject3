using System;
using _Members.NKY.Scripts;
using _Members.PJH._01.Script.Skill;
using _TevLib.ModuleSystem;
using _TevLib.ServiceLocatorSystem;
using _TevLib.ServiceLocatorSystem.PoolService;
using UnityEngine;

namespace _Members.PJH._01.Script
{
    public class SkillModule : MonoModule 
    {
        
        [SerializeField] private SkillDataSO skillData;
        private IPoolingService _poolService;
        public bool CanUseSkill { get; private set;}
        private float lastSkillTime;
        private float currentTime;

        private void Start()
        {
            _poolService = ServiceLocator.GetService<IPoolingService>();
        }

        private void Update()
        {
            currentTime += Time.deltaTime;
            
            if (currentTime >= lastSkillTime + skillData.coolTime)
            {
                CanUseSkill = true;
            }
            else
            {
                CanUseSkill = false;
            }
                
        }

        public void Skill()
        {
            lastSkillTime = Time.time;
            AbstractSkill skill = _poolService.Pop(skillData.skillPoolItem).GameObject.GetComponent<AbstractSkill>();
            skill.gameObject.transform.position = transform.position;
            skill.UseSkill();
        }

    }
}
