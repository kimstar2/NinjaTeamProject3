using System;
using Unity.Behavior;

[BlackboardEnum]
public enum EnemyState
{
	IDLE,
	CHASE,
	ATTACK,
	HIT,
	DEAD
}
