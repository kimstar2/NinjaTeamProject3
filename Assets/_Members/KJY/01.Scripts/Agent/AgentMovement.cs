using _TevLib.ModuleSystem;
using UnityEngine;

namespace _Members.KJY._01.Scripts.Agent
{
    public class AgentMovement : MonoModule , IMoveable
    {
        public Rigidbody2D RbCompo { get; private set; }
        public Vector2 MoveDir { get; private set; }
        private float _multi = 1f;
        private float _speed = 1f;
        
        public override void Init(ModuleOwner owner)
        {
            base.Init(owner);
            RbCompo =  Owner.GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            RbCompo.linearVelocity = MoveDir * (_speed * _multi);
        }
        
        public void SetDirection(Vector2 moveDir)
        {
            MoveDir = moveDir;
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