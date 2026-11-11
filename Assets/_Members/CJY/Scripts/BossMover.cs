using System;
using UnityEngine;

namespace _Members.CJY.Scripts
{
    public class BossMover : MonoBehaviour
    {
        private Rigidbody _rigid;

        private void Awake()
        {
            _rigid = GetComponent<Rigidbody>();
        }

        public void Move()
        {
            
        }
    }
}