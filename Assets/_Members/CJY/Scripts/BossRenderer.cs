using System;
using UnityEngine;

namespace _Members.CJY.Scripts
{
    public class BossRenderer : MonoBehaviour
    {
        private SpriteRenderer _renderer;

        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
        }
    }
}