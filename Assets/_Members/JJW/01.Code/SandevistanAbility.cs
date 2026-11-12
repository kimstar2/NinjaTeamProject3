using UnityEngine;

namespace _Members.JJW._01.Code
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(AfterimageTrailJJW))]
    public class SandevistanAbility : MonoBehaviour
    {
        [Tooltip("발동 중 적 탄환의 이동 배율입니다. 0은 정지, 1은 원래 속도입니다.")]
        [SerializeField, Range(0f, 1f)] private float enemyBulletSpeedMultiplier = 0.02f;

        private AfterimageTrailJJW _trail;
        private double _endTime = double.NegativeInfinity;

        public bool IsActive => isActiveAndEnabled && Time.timeAsDouble < _endTime;

        public float EnemyBulletSpeedMultiplier =>
            IsActive
                ? Mathf.Clamp01(enemyBulletSpeedMultiplier)
                : 1f;

        private void Awake()
        {
            _trail = GetComponent<AfterimageTrailJJW>();
        }

        public void Activate()
        {
            if (!isActiveAndEnabled || _trail == null || IsActive)
                return;

            // 잔상 생성 시간까지만 적용하고, 종료 후 페이드 시간은 포함하지 않는다.
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
