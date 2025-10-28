using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using UnityEngine.AI;
using Action = Unity.Behavior.Action;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "NavigateMove", story: "[Self] is Move to [MovePos] with [Speed]", category: "Action", id: "6680395592cf3eccc2eaf2cd03ee91dc")]
public partial class NavigateMoveAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<Vector3> MovePos;
    [SerializeReference] public BlackboardVariable<float> Speed;

    NavMeshAgent agent;

    protected override Status OnStart()
    {
        if (Self.Value == null)
            return Status.Failure;
        if(Self.Value.TryGetComponent<IMovable>(out var movable))
            movable.MoveTo(MovePos, Speed); 
        agent = Self.Value.GetComponent<NavMeshAgent>();
        if (agent == null)

        agent.isStopped = false;
        agent.speed = Speed.Value;
        agent.SetDestination(MovePos.Value);

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (agent == null)
            return Status.Failure;


        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
            {
                agent.isStopped = true;
                return Status.Success;
            }
        }

        return Status.Running;
    }

    protected override void OnEnd()
    {
    }
}

