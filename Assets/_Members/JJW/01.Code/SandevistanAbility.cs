using UnityEngine;

namespace _Members.JJW._01.Code
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(AfterimageTrailJJW))]
    public class SandevistanAbility : MonoBehaviour
    {
        [SerializeField, Range(0f, 1f)] private float enemyBulletSpeedMultiplier = 0.02f;

        private AfterimageTrailJJW _trail;
        private double _endTime = double.NegativeInfinity;

        public bool IsActive => isActiveAndEnabled && Time.timeAsDouble < _endTime;

        public float EnemyBulletSpeedMultiplier => IsActive ? Mathf.Clamp01(enemyBulletSpeedMultiplier) : 1f;

        private void Awake()
        {
            _trail = GetComponent<AfterimageTrailJJW>();
        }

        public void Activate()
        {
            if (!isActiveAndEnabled || _trail == null || IsActive)
                return;

            _endTime = Time.timeAsDouble + _trail.EmissionDuration;
        }

        public void Deactivate()
        {
            _endTime = double.NegativeInfinity;
        }

        private void OnDisable()
        {
            Deactivate();
        }
    }
}
