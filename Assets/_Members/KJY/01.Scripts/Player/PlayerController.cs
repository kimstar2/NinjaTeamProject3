using System;
using _Members.KJY._01.Scripts.Agent;
using _Members.KJY._01.Scripts.MainSystem;
using _TevLib.ModuleSystem;
using UnityEngine;

namespace _Members.KJY._01.Scripts.Player
{
    public class PlayerController : ModuleOwner
    {
        [field:SerializeField] public PlayerInputSO PlayerInput {get; private set;}

        [Header("")]
        [SerializeField] private float baseSpeed;
        
        public IMoveable Mover { get; private set; }

        protected override void InitializeModules()
        {
            base.InitializeModules();
            Mover = GetModule<IMoveable>();
        }

        public void ValidateData()
        {
            // Mover.SetSpeed();
        }

        private void Update()
        {
            Mover.SetDirection(PlayerInput.InputDir);
        }

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
