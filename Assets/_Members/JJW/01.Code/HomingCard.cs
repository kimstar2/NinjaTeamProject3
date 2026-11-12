using UnityEngine;

namespace _Members.JJW._01.Code
{
    [RequireComponent(typeof(Rigidbody2D), typeof(PolygonCollider2D))]
    public class HomingCard : MonoBehaviour
    {
        public enum FlightMode
        {
            Homing,
            Straight
        }

        [SerializeField] private FlightMode flightMode = FlightMode.Homing;
        [Tooltip("직선탄이 사라지기 전까지 이동하는 최대 거리입니다.")]
        [SerializeField, Min(0.1f)] private float straightTravelDistance = 30f;
        [SerializeField, Min(0.1f)] private float speed = 14f;
        [SerializeField, Min(0f)] private float stunDuration = 2f;

        private Rigidbody2D _rb;
        private Collider2D _target;
        private SandevistanAbility _targetAbility;
        private Vector2 _launchDirection;
        private float _remainingTravelDistance;
        private bool _finished;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        public void Launch(Collider2D target)
        {
            _target = target;
            if (_target == null)
            {
                Finish();
                return;
            }

            _targetAbility = _target.GetComponentInParent<SandevistanAbility>();

            Vector2 direction = (Vector2)_target.bounds.center - _rb.position;
            if (direction.sqrMagnitude <= 0.000001f)
            {
                HitTarget();
                return;
            }

            // 직선탄은 발사 순간의 방향을 계속 사용한다.
            _launchDirection = direction.normalized;
            _remainingTravelDistance = Mathf.Max(0.1f, straightTravelDistance);
            _rb.rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        }

        private void FixedUpdate()
        {
            if (_finished)
                return;

            if (_target == null || !_target.enabled || !_target.gameObject.activeInHierarchy)
            {
                Finish();
                return;
            }

            float speedMultiplier = _targetAbility != null
                ? _targetAbility.EnemyBulletSpeedMultiplier
                : 1f;
            float moveDistance = speed * speedMultiplier * Time.fixedDeltaTime;

            if (flightMode == FlightMode.Straight)
            {
                MoveStraight(moveDistance);
                return;
            }

            Vector2 toTarget = (Vector2)_target.bounds.center - _rb.position;
            float distance = toTarget.magnitude;
            if (distance <= 0.0001f)
            {
                HitTarget();
                return;
            }

            Vector2 direction = toTarget.normalized;
            moveDistance = Mathf.Min(moveDistance, distance);
            _rb.MovePosition(_rb.position + direction * moveDistance);

            // 발동 중에는 현재 각도를 유지하고, 남아 있는 회전 속도도 없앤다.
            if (_targetAbility != null && _targetAbility.IsActive)
                _rb.angularVelocity = 0f;
            else
                _rb.MoveRotation(Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f);
        }

        private void MoveStraight(float moveDistance)
        {
            if (_remainingTravelDistance <= 0f)
            {
                Finish();
                return;
            }

            moveDistance = Mathf.Min(moveDistance, _remainingTravelDistance);
            _rb.MovePosition(_rb.position + _launchDirection * moveDistance);
            _rb.angularVelocity = 0f;
            _remainingTravelDistance -= moveDistance;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other == _target)
                HitTarget();
        }

        private void HitTarget()
        {
            if (_finished)
                return;

            Finish();
        }

        private void Finish()
        {
            _finished = true;
            Destroy(gameObject);
        }
    }
}

