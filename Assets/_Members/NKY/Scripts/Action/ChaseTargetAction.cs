using System;
using _Members.NKY.Scripts;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Chase Target", story: "[Self] Chase to [Target]", category: "Action", id: "5204fa02fc73e3d0506b078ac2547c14")]
public partial class ChaseTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> Target;

    private AbstractEnemy _agent;
    private GameObject _target;

    protected override Status OnStart()
    {

        _agent = Self?.Value.GetComponent<AbstractEnemy>();
        
        _target = Target?.Value;
        
        if(_target == null || _agent == null)
            return Status.Failure;
        
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        Vector2 moveDir = _target.transform.position - _agent.transform.position;
        // a*활용한 이동 함수 실행

        return Status.Success;
    }

    protected override void OnEnd()
    {
        //이동 멈추는거 실행
    }
}

