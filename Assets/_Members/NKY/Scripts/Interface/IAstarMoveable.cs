using System.Collections.Generic;
using System.Threading.Tasks;
using _Members.KJY._01.Scripts.Agent;
using _TevLib.TileAstar;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Members.NKY.Scripts.Interface
{
    public interface IAstarMoveable : IMoveable
    {
        float MoveSpeed { get; }
        PathAgent PathCompo { get; }
        UniTask<bool> StartAstarMove(Vector2 destination);
        
        void StopAstarMove();
    }
}