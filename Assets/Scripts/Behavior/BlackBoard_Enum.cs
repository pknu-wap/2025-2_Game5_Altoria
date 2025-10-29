using Unity.Behavior;
using UnityEngine;

[BlackboardEnum]
public enum AIState
{
    Idle,
    Patrol,
    Ambient,
    Dead
}
[BlackboardEnum]
public enum InteractState
{
    None,
    Interacting,
}
[BlackboardEnum]
public enum EnemyState
{
    Tracing,
    Escaping,
    Attack
}