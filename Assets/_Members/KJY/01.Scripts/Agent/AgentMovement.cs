using _TevLib.ModuleSystem;
using UnityEngine;

namespace _Members.KJY._01.Scripts.Agent
{
    public class AgentMovement : MonoModule , IMoveable
    {
        public Rigidbody2D RbCompo { get; private set; }
        private Vector2 _moveDir;
        private float _multi = 1f;
        private float _speed;
        
        public override void Init(ModuleOwner owner)
        {
            base.Init(owner);
            RbCompo =  Owner.GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            RbCompo.linearVelocity = _moveDir * (_speed * _multi);
        }
        
        public void SetDirection(Vector2 moveDir)
        {
            _moveDir = moveDir;
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