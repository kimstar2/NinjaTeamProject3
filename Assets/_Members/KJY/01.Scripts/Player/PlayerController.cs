using System;
using _Members.KJY._01.Scripts.MainSystem;
using UnityEngine;

namespace _Members.KJY._01.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        [field:SerializeField] public PlayerInputSO PlayerInput {get; private set;}

        private void OnEnable()
        {
            PlayerInput.SetEnable();
        }

        private void OnDisable()
        {
            PlayerInput.SetDisable();
        }
    }
}
