using UnityEngine;

namespace _Members.JJW._01.Code
{
    public class EnemyTestJJW : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField, Min(0f)] private float turnSpeed = 180f;

    [SerializeField, Min(0f)] private float detectRange;
    [SerializeField, Min(0.1f)] private float attackCooldown = 10f;
    [SerializeField] private HomingCard cardPrefab;
    public LayerMask playerMask;

    [Header("Range Display")]
    [SerializeField, Min(0.01f)] private float rangeLineWidth = 0.06f;
    [SerializeField] private Color idleRangeColor = new Color(0.65f, 0.85f, 1f, 0.8f);
    [SerializeField] private Color detectedRangeColor = new Color(1f, 0.45f, 0.12f, 1f);

    private const int RangeSegments = 96;
    private readonly Vector3[] _rangePoints = new Vector3[RangeSegments];
    private LineRenderer _detectLine;
    private SpriteRenderer _bodyRenderer;
    private bool _playerInRange;
    private float _cooldownRemaining;

    private void Awake()
    {
        _bodyRenderer = GetComponent<SpriteRenderer>();

        _detectLine = GetComponent<LineRenderer>();
        if (_detectLine == null)
            _detectLine = gameObject.AddComponent<LineRenderer>();

        _detectLine.sharedMaterial = _bodyRenderer.sharedMaterial;
        _detectLine.useWorldSpace = true;
        _detectLine.loop = true;
        _detectLine.positionCount = RangeSegments;
        _detectLine.widthCurve = AnimationCurve.Constant(0f, 1f, 1f);
        _detectLine.numCornerVertices = 2;
        _detectLine.alignment = LineAlignment.View;
        _detectLine.sortingLayerID = _bodyRenderer.sortingLayerID;
        _detectLine.sortingOrder = _bodyRenderer.sortingOrder - 1;
        DrawDetectRange();
    }

    private void Update()
    {
        UpdateDetectionAndAttack(Time.deltaTime);
        DrawDetectRange();
    }

    private void UpdateDetectionAndAttack(float deltaTime)
    {
        Collider2D player = DetectPlayer();
        _playerInRange = player != null;

        SandevistanAbility ability = player != null ? player.GetComponentInParent<SandevistanAbility>() : null;

        if (ability != null && ability.IsActive)
            return;

        _cooldownRemaining = Mathf.Max(0f, _cooldownRemaining - deltaTime);
        if (!_playerInRange)
            return;

        target = player.attachedRigidbody != null ? player.attachedRigidbody.transform : player.transform;
        RotateTowardsPlayer(deltaTime);

        if (_cooldownRemaining <= 0f)
            Attack(player);
    }

    private void Attack(Collider2D player)
    {
        if (cardPrefab == null || player == null)
            return;

        Vector3 spawnPosition = transform.position + transform.right * 0.8f;
        HomingCard card = Instantiate(cardPrefab, spawnPosition, Quaternion.identity);
        card.Launch(player);

        _cooldownRemaining = Mathf.Max(0.1f, attackCooldown);
    }

    private void DrawDetectRange()
    {
        float radius = Mathf.Max(0f, detectRange);
        Vector3 center = transform.position;
        for (int i = 0; i < RangeSegments; i++)
        {
            float angle = 2f * Mathf.PI * i / RangeSegments;
            _rangePoints[i] = center + new Vector3(radius * Mathf.Cos(angle), radius * Mathf.Sin(angle), 0f);
        }

        _detectLine.SetPositions(_rangePoints);
        _detectLine.widthMultiplier = Mathf.Max(0.01f, rangeLineWidth);
        Color rangeColor = _playerInRange ? detectedRangeColor : idleRangeColor;
        _detectLine.startColor = rangeColor;
        _detectLine.endColor = rangeColor;
        _detectLine.enabled = radius > 0f;
    }

    private void RotateTowardsPlayer(float deltaTime)
    {
        if (target == null || turnSpeed <= 0f || deltaTime <= 0f)
            return;

        Vector2 toTarget = target.position - transform.position;
        if (toTarget.sqrMagnitude < 0.000001f)
            return;

        float currentAngle = transform.eulerAngles.z;
         float radians = currentAngle * Mathf.Deg2Rad;
        Vector2 currentDirection = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));
        Vector2 targetDirection = toTarget.normalized;

        float crossZ = currentDirection.x * targetDirection.y - currentDirection.y * targetDirection.x;

        float targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        float remainingAngle = Mathf.DeltaAngle(currentAngle, targetAngle);

        if (Mathf.Abs(remainingAngle) < 0.001f)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, targetAngle);
            return;
        }

        float turnDirection = Mathf.Abs(crossZ) > 0.000001f ? Mathf.Sign(crossZ) : Mathf.Sign(remainingAngle);

        float turnAmount = Mathf.Min(turnSpeed * deltaTime, Mathf.Abs(remainingAngle));
        float nextAngle = currentAngle + turnDirection * turnAmount;
        transform.rotation = Quaternion.Euler(0f, 0f, nextAngle);
    }

    public Collider2D DetectPlayer()
    {
        if (detectRange <= 0f)
            return null;

        return Physics2D.OverlapCircle(transform.position, detectRange, playerMask);
    }

    private void OnDisable()
    {
        if (_detectLine != null)
            _detectLine.enabled = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (Application.isPlaying)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Mathf.Max(0f, detectRange));
    }
}
}

