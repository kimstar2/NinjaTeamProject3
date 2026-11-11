using System;
using _Members.KJY._01.Scripts.Agent;
using _Members.KJY._01.Scripts.Agent.Hand;
using _Members.KJY._01.Scripts.MainSystem;
using _TevLib.ModuleSystem;
using DG.Tweening.Core.Easing;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Members.KJY._01.Scripts.Player
{
    public class PlayerController : ModuleOwner
    {
        [field: SerializeField] public PlayerInputSO PlayerInput { get; private set; }

        [Header("Movement Setting")]
        [SerializeField] private float baseSpeed;

        [field: Header("Hand Setting")] 
        [field: SerializeField] public float rotAcc;
        [field: SerializeField] public AgentHand LHand { get; private set; }
        [field: SerializeField] public AgentHand RHand { get; private set; }

        public AgentHandler Handler { get; private set; }
        public IMoveable Mover { get; private set; }
        public IDash Dasher {get; private set;}
        
        public Camera Camera { get; private set; }

        
        private Vector2 _mPosDir;
        private bool CanMove => !Dasher.IsDashing;
        
        protected override void InitializeModules()
        {
            base.InitializeModules();

            Camera = Camera.main;

            Mover = GetModule<IMoveable>();
            Handler = GetModule<AgentHandler>();
            Dasher = GetModule<IDash>();

            ValidateData();
        }

        private void ValidateData()
        {
            Mover.SetSpeed(baseSpeed);
        }

        private void Update()
        {
            if (CanMove)
                Mover.SetDirection(PlayerInput.InputDir);

            _mPosDir = ((Vector3)WorldMousePosition() - transform.position).normalized;
            float zAngle = AgentUtil.GetZAngle(_mPosDir);
            float lerpAngle = Mathf.LerpAngle(Handler.transform.localEulerAngles.z, zAngle,1 - Mathf.Pow(1 - Time.fixedDeltaTime * rotAcc, 3));
            // 1 - mathf.Pow ~~ 이거는 easeOutCubic 함수임
            Handler.SetZAngle(lerpAngle);
        }

        private void OnEnable()
        {
            PlayerInput.SetEnable();

            PlayerInput.OnAttackPressed += HandleAttack;
            PlayerInput.OnLShiftPressed += DashWithMoveDir;
            PlayerInput.OnJumpPressed += DashWithMoveDir;
            PlayerInput.OnRClickPressed += DashWithMouseDir;
        }
        private void OnDisable()
        {
            PlayerInput.OnAttackPressed -= HandleAttack;
            PlayerInput.OnLShiftPressed -= DashWithMoveDir;
            PlayerInput.OnJumpPressed -= DashWithMoveDir;
            PlayerInput.OnRClickPressed -= DashWithMouseDir;

            PlayerInput.SetDisable();
        }

        #region Handle
        
        private bool _lastIsRight = true;
        private void HandleAttack(bool attacking)
        {
            if (attacking)
            {
                _lastIsRight = !_lastIsRight;
                if (_lastIsRight)
                    LHand.Attack(WorldMousePosition());
                else
                    RHand.Attack(WorldMousePosition());
            }
        }
        
        private void DashWithMoveDir()
        {
            Dasher.Dash(Mover.MoveDir);
        }
        
        private void DashWithMouseDir()
        {
            Dasher.Dash(_mPosDir);
        }


        #endregion

        #region Helper
        
        private Vector2 WorldMousePosition()
            => Camera.ScreenToWorldPoint(PlayerInput.LookPos);
        
        #endregion
    }
}