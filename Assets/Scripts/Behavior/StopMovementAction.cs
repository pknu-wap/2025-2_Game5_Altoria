using GameInteract;
using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using UnityEngine.AI;
using Action = Unity.Behavior.Action;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "StopMovement", story: "[Self] StopMove", category: "Action", id: "2d233d14d8fba7c1efbaa9db2a7254d7")]
public partial class StopMovementAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;


    private NavMeshAgent agent;

    protected override Status OnStart()
    {
        

        agent = Self.Value.GetComponent<NavMeshAgent>();
        if (agent == null)
            return Status.Failure;

    
        agent.isStopped = true;
        agent.ResetPath();

        if (Self.Value.TryGetComponent<IMovable>(out var movable))
            movable.Stop();

        return Status.Success;
    }


    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

