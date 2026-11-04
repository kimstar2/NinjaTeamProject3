using _TevLib.ModuleSystem;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Members.KJY._01.Scripts
{
    public class AgentMovement : MonoModule
    {
        public Rigidbody2D RbCompo { get; private set; } 
        public override void Init(ModuleOwner owner)
        {
            base.Init(owner);
            Owner.GetComponent<Rigidbody2D>();
        }
    }
}