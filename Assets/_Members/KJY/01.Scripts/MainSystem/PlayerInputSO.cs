using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Members.KJY._01.Scripts.MainSystem
{
    [CreateAssetMenu(fileName = "PlayerInput data", menuName = "KJY/MainSystem/PlayerInput data", order = 0)]
    public class PlayerInputSO : ScriptableObject, Controls.IPlayerActions
    {
        #region Default Setting

        private Controls _controls;
        public void SetEnable()
        {
            if (_controls == null)
            {
                _controls = new Controls();
                _controls.Player.SetCallbacks(this);
            }
            _controls.Player.Enable();
        }
        public void SetDisable() => _controls?.Player.Disable();

        #endregion

        public Vector3 InputDir {get; private set;}
        public Vector3 LookPos {get; private set;}

        public event Action<bool> OnAttackPressed;
        public event Action OnJumpPressed;
        public event Action OnSprintPressed;

        public void OnMove(InputAction.CallbackContext context)
        {
            InputDir = context.ReadValue<Vector2>();
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            LookPos = context.ReadValue<Vector2>();
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (context.performed)
                OnAttackPressed?.Invoke(true);
            if (context.canceled)
                OnAttackPressed?.Invoke(false);
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.performed)
                OnJumpPressed?.Invoke();
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            if (context.performed)
                OnSprintPressed?.Invoke();
        }
    }
}