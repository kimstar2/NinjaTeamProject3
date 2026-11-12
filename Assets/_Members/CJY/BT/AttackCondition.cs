using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "Attack", story: "[Boss] Attack [Target]", category: "Conditions", id: "b5e899e40c26f1fe3bb40a71cde465e1")]
public partial class AttackCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Boss;
    [SerializeReference] public BlackboardVariable<Collider2D> Target;

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
