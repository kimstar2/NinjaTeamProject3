using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Members.CJY.Scripts
{
    public class BossRenderer : MonoBehaviour
    {
        private SpriteRenderer _renderer;
        private Transform _parent;

        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
            _parent = GetComponentInParent<Transform>();
        }

        public void Flip(Vector2 flipDir)
        {
            Vector2 dir = flipDir;
            if(_parent.position.x > dir.x)
                _parent.rotation = Quaternion.Euler(0, 180, 0);
            else 
                _parent.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}