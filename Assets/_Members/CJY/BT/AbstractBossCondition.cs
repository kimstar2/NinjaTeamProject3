using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "AbstractBoss", story: "[Boss]", category: "Conditions", id: "9c2d7daade3a2aefd1ef5b8f79f89d98")]
public partial class AbstractBossCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Boss;

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
