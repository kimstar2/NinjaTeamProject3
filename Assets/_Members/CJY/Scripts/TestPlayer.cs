using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Members.CJY.Scripts
{
    public class TestPlayer : MonoBehaviour
    {
        [SerializeField] private float speed = 3f;
        [SerializeField] private float duration = 0.2f;
        private Vector2 _moveDir;
        private Camera _cam;

        private void Awake()
        {
            _cam = Camera.main;
        }

        private void Update()
        {
            _moveDir = Mouse.current.position.ReadValue();
            Vector2 worldPos = _cam.ScreenToWorldPoint(_moveDir);
            transform.position = Vector2.Lerp(transform.position, worldPos, duration * Time.deltaTime);
        }
    }
}