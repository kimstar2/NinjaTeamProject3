using System;
using System.Threading;
using _Members.KJY._01.Scripts.Agent;
using _TevLib.ModuleSystem;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Members.NKY.Scripts
{
    public abstract class AbstractEnemy : AbstractAgent
    {
        public AstarMovementModule MoveCompo { get; private set; }

        protected override void InitializeModules()
        {
            base.InitializeModules();
            MoveCompo = GetModule<AstarMovementModule>();
        }
    }
}
