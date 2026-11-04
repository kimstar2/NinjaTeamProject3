using System;
using _Members.CJY.Scripts;
using UnityEngine;

public class AbstractBoss : MonoBehaviour
{
    [Header("세팅")] 
    public BossMover MoveCompo { get; private set; }
    public BossRenderer RenderCompo {get; private set; }

    private void Awake()
    {
        MoveCompo = GetComponent<BossMover>();
        RenderCompo = GetComponentInChildren<BossRenderer>();
    }

    public virtual void Update()
    {
        MoveCompo.Move();
    }
}
