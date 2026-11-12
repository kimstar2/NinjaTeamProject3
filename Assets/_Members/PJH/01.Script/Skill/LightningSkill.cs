using _TevLib.ServiceLocatorSystem;
using _TevLib.ServiceLocatorSystem.PoolService;
using UnityEngine;

namespace _Members.PJH._01.Script.Skill
{
    public class LightningSkill : AbstractSkill
    {
        
        public override void Init(SkillDataSO skillData)
        {
            base.Init(skillData);
        }
        
        public override void UseSkill()
        {
            //생성하고 에니메이터도 받아와

            IPoolable skill = ServiceLocator.GetService<IPoolingService>().Pop(SkillData.skillItem);
            skill.GameObject.transform.SetPositionAndRotation(transform.position, transform.rotation);
        }
        
        
        public void EndDestroy()
        {
            ServiceLocator.GetService<IPoolingService>().Push(this);
        }
    }
}
