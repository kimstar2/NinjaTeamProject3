using System;
using _Members.KJY._01.Scripts.Agent;
using UnityEngine;

public class EnemyMovement : MonoBehaviour, IMoveable
{
    public Rigidbody2D RbCompo { get; private set; }

    public Vector2 MoveDir { get; private set; }
    private void Awake()
    {
        RbCompo = GetComponent<Rigidbody2D>();
    }

    public void SetDirection(Vector2 moveDir)
    {
        MoveDir = moveDir;
    }

    public void SetMulti(float multi)
    {
        
    }

    public void SetSpeed(float speed)
    {
        throw new System.NotImplementedException();
    }
}
