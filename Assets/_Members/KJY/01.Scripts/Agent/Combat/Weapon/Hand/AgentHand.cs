using System;
using _Members.KJY._01.Scripts.Object.Detector;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;

namespace _Members.KJY._01.Scripts.Agent.Combat.Weapon.Hand
{
    public class AgentHand : MonoBehaviour
    {
        [field:SerializeField] public bool IsRightHand {get; private set;}
        [SerializeField , MinMaxRangeSlider(1f,10f)] private Vector2 attackRange;
        [SerializeField] private BaseOverlap attackOverlap;
        
        [Header("Tween Setting")]   
        [SerializeField] private float duration;
        [SerializeField] private Ease raiseEase , returnEase;

        [Header("Event Setting")]
        [SerializeField] private UnityEvent onHit;
        
        private Vector2 _originPos;
        private Sequence _attackSequence;
        
        public event Action OnAttackMotionEnd;
        public event Action OnAttackEnd;

        private void Awake()
        {
            _originPos = transform.localPosition;
        }

        public void Init() { }

        public void Attack(Vector2 pos)
        {
            _attackSequence?.Kill();
            
            float posMagnitude = (pos - (Vector2)transform.position).magnitude;
            
            _attackSequence = DOTween.Sequence();
            transform.localPosition = _originPos;
            _attackSequence.Append(transform.
                DOLocalMove(Vector2.right * Mathf.Clamp(posMagnitude,attackRange.x,attackRange.y),duration)).
                SetEase(raiseEase);

            _attackSequence.AppendCallback(AttackTarget);
            
            _attackSequence.Append(transform.
                DOLocalMove(_originPos ,duration)).
                SetEase(returnEase).
                OnComplete(() => OnAttackMotionEnd?.Invoke());
        }

        private void AttackTarget()
        {
            OnAttackEnd?.Invoke();
            
            attackOverlap.Detect();
            
            float theta = transform.eulerAngles.z * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(Mathf.Cos(theta), Mathf.Sin(theta));
            
            for (int i = 0; i < attackOverlap.Count; i++)
            {
                if (attackOverlap.Result[i].TryGetComponent(out IDamageable damageable))
                {
                    damageable.ApplyDamage(new DamageData(2f,direction));
                    onHit?.Invoke();
                }
            }
        }
    }
}