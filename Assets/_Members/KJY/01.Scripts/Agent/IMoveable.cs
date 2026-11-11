using UnityEngine;

namespace _Members.KJY._01.Scripts.Agent
{
    public interface IMoveable
    {
        Rigidbody2D RbCompo { get; }
        Vector2 MoveDir { get; } 
        void SetDirection(Vector2 moveDir);
        void SetMulti(float multi);
        void SetSpeed(float speed);
    }
}