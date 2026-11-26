using System;
using _TevLib.ServiceLocatorSystem;
using _TevLib.ServiceLocatorSystem.PoolService;
using UnityEngine;

namespace _Members.PJH._01.Script.Skill
{
    public abstract class AbstractSkill : MonoBehaviour, IPoolable
    {
        [field:SerializeField] public SkillDataSO SkillData { get; private set; }
        [field: SerializeField] public PoolItemSO Item {get; private set;} 
        protected Animator animator;
        
        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public virtual void UseSkill()
        {
            //IPoolable skill = ServiceLocator.GetService<IPoolingService>().Pop(Item);
            //skill.GameObject.transform.SetPositionAndRotation(transform.position, transform.rotation);

            SkillAnimation();

        }

        public GameObject GameObject => gameObject;
        public void ResetItem()
        {
        }
        
        public virtual void EndDestroy()
        {
            ServiceLocator.GetService<IPoolingService>().Push(this);
        }

        public void SkillAnimation()
        {
            animator.Play(SkillData.skillAnimHash.HashValue);
        }
    }
}