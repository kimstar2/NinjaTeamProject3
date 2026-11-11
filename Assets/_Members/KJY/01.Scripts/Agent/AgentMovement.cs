using _TevLib.ModuleSystem;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace _Members.KJY._01.Scripts.Agent
{
    public class AgentMovement : MonoModule, IMoveable , IVelocityAddable
    {
        [Header("Acceleration Setting")] [SerializeField]
        private bool useAcceleration;

        [ShowIf("useAcceleration")] [SerializeField]
        private float acceleration = 40f;

        private float _multi = 1f;
        private float _speed = 1f;

        public Rigidbody2D RbCompo { get; private set; }
        public Vector2 MoveDir { get; private set; }
        public Vector2 AddVelocity { get; private set; }

        public override void Init(ModuleOwner owner)
        {
            base.Init(owner);
            RbCompo = Owner.GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            RbCompo.linearVelocity = Vector2.MoveTowards(RbCompo.linearVelocity, MoveDir * (_speed * _multi) + AddVelocity,
                acceleration * Time.fixedDeltaTime);
        }

        public void SetDirection(Vector2 moveDir)
        {
            MoveDir = moveDir;
        }
        
        public void SetAddVelocity(Vector2 velocity)
        {
            AddVelocity = velocity;
        }

        public void SetMulti(float multi)
        {
            _multi = multi;
        }

        public void SetSpeed(float speed)
        {
            _speed = speed;
        }
    }
}