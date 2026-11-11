using UnityEngine;

namespace _Members.KJY._01.Scripts.Agent
{
    public interface IVelocityAddable
    {
        Vector2 AddVelocity { get; }
        void SetAddVelocity(Vector2 velocity);
    }
}