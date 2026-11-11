using System.Collections.Generic;
using _Members.KJY._01.Scripts.Agent;
using UnityEngine;

namespace _Members.NKY.Scripts
{
    public interface IToMoveable : IMoveable
    {
        void ToMove(Vector3[] movePath);
    }
}