using _Members.KJY._01.Scripts.Agent.Combat;
using UnityEngine;

namespace _Members.KJY._01.Scripts.Agent
{
    public class TestAgent : AbstractAgent
    {
        protected override void HandleDamageApplied(DamageData dmgData)
        {
            base.HandleDamageApplied(dmgData);
            Debug.Log(dmgData.Damage);
        }

        protected override void HandleDead() { }
    }
}