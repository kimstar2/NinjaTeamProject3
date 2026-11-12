using System;
using UnityEngine;

namespace _Members.CJY.Scripts
{
    public class BossMover : MonoBehaviour
    {
        private Rigidbody2D _rigid;
        private Vector2 _moveDir;
        private float _speed;

        private void Awake()
        {
            _rigid = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            _rigid.linearVelocity = _moveDir * _speed;
        }

        public void Move(Vector2 moveDir, float speed)
        {
            _moveDir = moveDir;
            _speed = speed;
        }

        public void StopMove()
        {
            _speed = 0;
        }
        
    }
}