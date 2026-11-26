using System;
using System.Threading;
using _TevLib.ModuleSystem;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Members.NKY.Scripts
{
    public abstract class AbstractEnemy : ModuleOwner
    {
        public EnemyMovementModule MoveCompo { get; private set; }

        protected override void InitializeModules()
        {
            base.InitializeModules();
            MoveCompo = GetModule<EnemyMovementModule>();
        }
    }
}
