using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;

namespace _Members.NKY.Scripts.Action
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "Chase Target", story: "[Self] Chase to [Target]", category: "Action", id: "5204fa02fc73e3d0506b078ac2547c14")]
    public partial class ChaseTargetAction : Unity.Behavior.Action
    {
        [SerializeReference] public BlackboardVariable<GameObject> Self;
        [SerializeReference] public BlackboardVariable<GameObject> Target;

        private AbstractEnemy _agent;
        private GameObject _target;
    
        private UniTask<bool> _moveTask;
        private CancellationTokenSource _cts;

        protected override Status OnStart()
        {
            _agent = Self?.Value.GetComponent<AbstractEnemy>();
        
            _target = Target?.Value;
        
            if(_target == null || _agent == null)
                return Status.Failure;
        
            _cts = new CancellationTokenSource();
            Vector2 targetPos = _target.transform.position;

            _moveTask = _agent.MoveCompo.StartAstarMove(targetPos);
        
            return Status.Running;
        }

        protected override Status OnUpdate()
        {
            if (!_moveTask.Status.IsCompleted())
            {
                return Status.Running;
            }
        
            bool isSuccess = _moveTask.GetAwaiter().GetResult();

            return isSuccess ? Status.Success : Status.Failure;
        }

        protected override void OnEnd()
        {
            _agent.MoveCompo.StopAstarMove();
        
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
        }
    }
}

