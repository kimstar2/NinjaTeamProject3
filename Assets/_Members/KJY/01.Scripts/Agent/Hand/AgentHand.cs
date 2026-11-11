using System;
using DG.Tweening;
using NaughtyAttributes;
using Unity.Cinemachine;
using UnityEngine;

namespace _Members.KJY._01.Scripts.Agent.Hand
{
    public class AgentHand : MonoBehaviour
    {
        [field:SerializeField] public bool IsRightHand {get; private set;}
        [SerializeField , MinMaxRangeSlider(1f,10f)] private Vector2 attackRange;

        [Header("Tween Setting")]   
        [SerializeField] private float duration;
        [SerializeField] private Ease raiseEase , returnEase;
        
        private Vector2 _originPos;
        private Sequence _attackSequence;
        
        private void Awake()
        {
            _originPos = transform.position;
        }

        public void Attack(Vector2 pos)
        {
            _attackSequence?.Kill();
            
            float posMagnitude = (pos - (Vector2)transform.position).magnitude;
            
            _attackSequence = DOTween.Sequence();
            _attackSequence.Append(transform.
                DOLocalMove(Vector2.right * Mathf.Clamp(posMagnitude,attackRange.x,attackRange.y),duration)).
                SetEase(raiseEase);
            
            _attackSequence.Append(transform.
                DOLocalMove(_originPos ,duration)).
                SetEase(returnEase);
        }
    }
}