using System;
using UnityEngine;

namespace _Members.KJY._01.Scripts.Object.Detector
{
    public abstract class BaseOverlap : MonoBehaviour
    {
        [field:SerializeField] public Transform TargetTransform {get; private set;}
        [field:SerializeField] public ContactFilter2D ContactFilter {get; private set;}
        [field:SerializeField] public int MaxCount {get; private set;}
        
        [Header("Debug Setting")]
        [SerializeField] protected bool useDebug = true; 
        [SerializeField] protected Color gizmoColor = Color.red; 
        public Collider2D[] Result { get; private set; }
        public int Count { get; protected set; }

        private void Awake()
        {
            Result = new Collider2D[MaxCount];
        }

        public abstract void Detect();
        
    }
}