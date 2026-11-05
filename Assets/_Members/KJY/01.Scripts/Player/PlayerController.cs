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

        [Header("Movement Setting")]
        [SerializeField] private float baseSpeed;
        
        public IMoveable Mover { get; private set; }
        public AgentHandler Handler { get; private set; }
        public Camera Camera { get; private set; }

        protected override void InitializeModules()
        {
            base.InitializeModules();
            
            Camera = Camera.main; 
            
            Mover = GetModule<IMoveable>();
            Handler = GetModule<AgentHandler>();
            
            ValidateData();
        }

        public void ValidateData()
        {
            Mover.SetSpeed(baseSpeed);
        }

        private void Update()
        {
            Mover.SetDirection(PlayerInput.InputDir);
            // Handler.SetZAngle();
        }

        private Vector2 WorldMousePosition()
        {
            return Camera.ScreenToWorldPoint(PlayerInput.LookPos);
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
