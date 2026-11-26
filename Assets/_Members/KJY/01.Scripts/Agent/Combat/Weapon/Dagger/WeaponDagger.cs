using _Members.KJY._01.Scripts.Object.Detector;
using _TevLib.ModuleSystem;
using DG.Tweening;
using UnityEngine;

namespace _Members.KJY._01.Scripts.Agent.Combat.Weapon.Dagger
{
    public class WeaponDagger : AbstractWeapon
    {
        [SerializeField] private Transform daggerPivot;
        [SerializeField] private CircleOverlap attackOverlap;

        [Header("Dagger Setting")]
        [SerializeField] private float duration;
        [SerializeField] private Ease ease;
        [SerializeField] private float swingAngle;       
        
        [Header("Dagger Setting")]
        [SerializeField] private float dashForce;
        [SerializeField] private float dashDur;
        [SerializeField] private Ease dashEase;
        
        
        private Tween _daggerTween; 
        private Tween _dashTween; 
        
        
        private int _currentDir = 1;
        // private IVelocityAddable _velocityAddable;

        public override void SetOwner(ModuleOwner moduleOwner)
        {
            base.SetOwner(moduleOwner);
            // _velocityAddable = moduleOwner.GetModule<IVelocityAddable>();
        }

        public override void Attack(AttackData data)
        {
            IsAttacking = true;
            CanChange = false;
            
            AttackTween();
            // AttackDashTween(data.WorldMousePos);
            attackOverlap.Detect();
            for (int i = 0; i < attackOverlap.Count; i++)
            {
                if (attackOverlap.Result[i].TryGetComponent(out IDamageable damageable))
                    damageable.ApplyDamage(new DamageData(2f,data.WorldMousePos));
            }
        }

        // private void AttackDashTween(Vector3 dataWorldMousePos)
        // {
        //     _dashTween?.Kill();
        //     
        //     Vector2 applyVector = (dataWorldMousePos - daggerPivot.position).normalized;
        //     Vector2 beforeVelocity = _velocityAddable.AddVelocity;
        //     Vector2 applyVelocity = applyVector * dashForce;
        //     _velocityAddable.SetAddVelocity(applyVelocity);
        //
        //     _dashTween = DOTween.To(() => applyVelocity,
        //         x =>
        //         {
        //             applyVelocity = x;
        //             _velocityAddable.SetAddVelocity(applyVelocity);
        //         }, Vector2.zero, dashDur).SetEase(dashEase);
        // }

        private void AttackTween()
        {
            _daggerTween?.Kill();
            _daggerTween = daggerPivot.
                DOLocalRotate(Vector3.forward * swingAngle * _currentDir, duration).
                SetEase(ease).
                OnComplete(() =>
                {
                    CanChange = true;
                    IsAttacking = false;
                });
            _currentDir *= -1;
        }
    }
}