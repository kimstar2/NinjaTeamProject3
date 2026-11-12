using _Members.KJY._01.Scripts.Object.Detector;
using DG.Tweening;
using UnityEngine;

namespace _Members.KJY._01.Scripts.Agent.Combat.Weapon.Dagger
{
    public class WeaponDagger : AbstractWeapon
    {
        [SerializeField] private Transform daggerPivot;
        [SerializeField] private CircleOverlap attackOverlap;

        [Header("Tween Setting")]
        [SerializeField] private float duration;
        [SerializeField] private Ease ease;
        [SerializeField] private float swingAngle;
        private Tween _daggerTween; 
        private int _currentDir = 1;
        
        public override void Init()
        {
        }

        public override void Attack(AttackData data)
        {
            IsAttacking = true;
            CanChange = false;
            
            AttackTween();

            attackOverlap.Detect();
            for (int i = 0; i < attackOverlap.Count; i++)
            {
                if (attackOverlap.Result[i].TryGetComponent(out IDamageable damageable))
                    damageable.ApplyDamage(new DamageData(2f,data.WorldMousePos));
            }
        }

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