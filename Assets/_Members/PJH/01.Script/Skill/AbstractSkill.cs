using System;
using _TevLib.ServiceLocatorSystem.PoolService;
using UnityEngine;

namespace _Members.PJH._01.Script.Skill
{
    public abstract class AbstractSkill : MonoBehaviour, IPoolable
    {
        protected SkillDataSO SkillData { get; private set; }
        
        public virtual void Init(SkillDataSO skillData)
        {
            SkillData = skillData;
        } 
        
        public abstract void UseSkill();

        [field: SerializeField] public PoolItemSO Item {get; private set;} 
        public GameObject GameObject => gameObject;
        public void ResetItem()
        {
        }
    }
}