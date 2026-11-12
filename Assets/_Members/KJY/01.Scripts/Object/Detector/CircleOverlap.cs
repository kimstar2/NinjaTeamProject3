using System;
using UnityEngine;

namespace _Members.KJY._01.Scripts.Object.Detector
{
    public class CircleOverlap : BaseOverlap
    {
        [SerializeField] private float radius;

        public override void Detect()
        {
            Count = Physics2D.OverlapCircle(TargetTransform.position, radius, ContactFilter, Result);
        }

#if UNITY_EDITOR

        private void OnDrawGizmosSelected()
        {
            if (!useDebug) return;
            if (TargetTransform == null) return;
            
            Gizmos.color = gizmoColor;
            Gizmos.DrawWireSphere(TargetTransform.position, radius);
        }

#endif
    }
}