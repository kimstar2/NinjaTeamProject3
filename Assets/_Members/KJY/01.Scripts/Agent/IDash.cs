using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Members.KJY._01.Scripts.Agent
{
    public interface IDash
    {
        public bool IsDashing { get; }
        public bool CanDash { get; }
        public void Dash(Vector2 targetDir);
    }
}