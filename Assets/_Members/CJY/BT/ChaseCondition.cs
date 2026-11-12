using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "Chase", story: "[Boss] Chase [Target]", category: "Conditions", id: "724140e46459641f1daaf4d2dce81346")]
public partial class ChaseCondition : Condition
{
    [SerializeReference] public BlackboardVariable<Transform> Boss;
    [SerializeReference] public BlackboardVariable<Transform> Target;

    public override bool IsTrue()
    {
        return true;
    }

    public override void OnStart()
    {
        
    }

    public override void OnEnd()
    {
    }
}
