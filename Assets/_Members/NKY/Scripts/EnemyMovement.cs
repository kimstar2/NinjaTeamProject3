using System.Collections.Generic;
using _Members.KJY._01.Scripts.Agent;
using UnityEngine;

namespace _Members.NKY.Scripts
{
    public class EnemyMovement : MonoBehaviour, IToMoveable
    {
        public Rigidbody2D RbCompo { get; private set; }

        public Vector2 MoveDir { get; private set; }
        public float Multi { get; private set; }
        public float Speed { get; private set; }
        private void Awake()
        {
            RbCompo = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            RbCompo.linearVelocity = MoveDir * (Speed * Multi);
        }

        public void SetDirection(Vector2 moveDir)
        {
            MoveDir = moveDir;
        }

        public void SetMulti(float multi)
        {
            Multi = multi;
        }

        public void SetSpeed(float speed)
        {
            Speed = speed;
        }

        public void ToMove(Vector3[] movePath)
        {
            
        }
    }
}
