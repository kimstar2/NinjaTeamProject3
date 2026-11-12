using System;
using _Members.CJY.Scripts;
using UnityEngine;

public class AbstractBoss : MonoBehaviour
{
    #region 컴포넌트 세팅
    public BossMover MoveCompo { get; private set; }
    public BossRenderer RenderCompo { get; private set; }
    public BossHealth HealthCompo { get; private set; }

    private Behaviour BT;

    #endregion

    #region SO데이터 받기
        [SerializeField] private BossDataSO bossData;
        
        private string _name => bossData.Name;
        private string _desc => bossData.Desc;
    
        private int _hp => bossData.HP; 
        private float _speed => bossData.Speed;
        
        private float _detectRange => bossData.DetectRange;
        private float _attackRange => bossData.AttackRange;
        private float _attakPower => bossData.AttackPower;
        private LayerMask _targetLayer => bossData.AttackTarget;

        private float _attackCool => bossData.AttackCool;
        private float _attack2Cool => bossData.Attack2Cool;
        private float _spAttackCool => bossData.SPAttackCool;
        private float _chargeCool => bossData.ChargeCool;
    #endregion
    
    private Collider2D _target;

    private void Awake()
    {
        MoveCompo = GetComponent<BossMover>();
        RenderCompo = GetComponentInChildren<BossRenderer>();
        HealthCompo = GetComponent<BossHealth>();
        BT = GetComponent<Behaviour>();
        
        HealthCompo.Init(_hp);
    }

    private void Update()
    {
        _target = GetTarget();
        
        if(_target != null) 
            RenderCompo.Flip(_target.transform.position);
    }

    private Collider2D GetTarget()
    {
        return Physics2D.OverlapCircle(transform.position, _detectRange, _targetLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _detectRange);
    }
}
