using _Members.KJY._01.Scripts.Agent;
using _Members.KJY._01.Scripts.Agent.Combat.Weapon;
using _Members.KJY._01.Scripts.MainSystem;
using _TevLib.ModuleSystem;
using UnityEngine;

namespace _Members.KJY._01.Scripts.Player
{
    public class PlayerController : AbstractAgent
    {
        [field: SerializeField] public PlayerInputSO PlayerInput { get; private set; }

        [Header("Movement Setting")]
        [SerializeField] private float baseSpeed;

        [Header("Hand Setting")] 
        [SerializeField] private float rotAcc;
        
        public AgentWeaponHandler WeaponHandler { get; private set; }
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
            WeaponHandler = GetModule<AgentWeaponHandler>();
            Dasher = GetModule<IDash>();

            ValidateData();
        }

        private void Update()
        {
            if (CanMove)
                Mover.SetDirection(PlayerInput.InputDir);

            SetAngle();
        }

        private void OnEnable()
        {
            PlayerInput.SetEnable();

            PlayerInput.OnAttackPressed += HandleAttack;
            PlayerInput.OnLShiftPressed += DashWithMoveDir;
            PlayerInput.OnJumpPressed += DashWithMoveDir;
            PlayerInput.OnRClickPressed += DashWithMouseDir;
            PlayerInput.OnWeaponChangeKeyPressed += HandleWeaponChanged;
        }

        private void OnDisable()
        {
            PlayerInput.OnAttackPressed -= HandleAttack;
            PlayerInput.OnLShiftPressed -= DashWithMoveDir;
            PlayerInput.OnJumpPressed -= DashWithMoveDir;
            PlayerInput.OnRClickPressed -= DashWithMouseDir;
            PlayerInput.OnWeaponChangeKeyPressed -= HandleWeaponChanged;

            PlayerInput.SetDisable();
        }

        private void ValidateData()
        {
            WeaponHandler.Init();
            Mover.SetSpeed(baseSpeed);
        }
        
        #region Handle
        
        private void HandleAttack(bool attacking)
        {
            WeaponHandler.Attack(new AttackData(WorldMousePosition(),attacking));
        }
        
        protected override void HandleDead()
        {
            
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
        
        private void SetAngle()
        {
            _mPosDir = ((Vector3)WorldMousePosition() - transform.position).normalized;
            float zAngle = AgentUtil.GetZAngle(_mPosDir);
            float lerpAngle = Mathf.LerpAngle(WeaponHandler.transform.localEulerAngles.z, zAngle,1 - Mathf.Pow(1 - Time.fixedDeltaTime * rotAcc, 3));
            // 1 - mathf.Pow ~~ 이거는 easeOutCubic 함수임
            WeaponHandler.SetZAngle(lerpAngle);
        }
        
        private void HandleWeaponChanged(int weaponIndex)
        {
            WeaponHandler.ChangeWeapon((WeaponType)weaponIndex);
        }
        
        #endregion
    }
}