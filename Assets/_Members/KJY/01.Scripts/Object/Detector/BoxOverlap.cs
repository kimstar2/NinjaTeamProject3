using UnityEngine;

namespace _Members.KJY._01.Scripts.Object.Detector
{
    public class BoxOverlap : BaseOverlap
    {
        [SerializeField] private Vector2 boxSize;

        public override void Detect()
        {
            Count = Physics2D.OverlapBox(TargetTransform.position,boxSize,0f, ContactFilter, Result);
        }

#if UNITY_EDITOR

        private void OnDrawGizmosSelected()
        {
            if (!useDebug) return;
            if (TargetTransform == null) return;
            
            Gizmos.color = gizmoColor;
            Gizmos.DrawWireCube(TargetTransform.position, boxSize);
        }

#endif
    }
}