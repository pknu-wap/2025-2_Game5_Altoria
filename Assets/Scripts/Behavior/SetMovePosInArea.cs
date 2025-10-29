using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SetMovePosInArea", story: "Set [MovePos] in [Area] with [PatrolRadius]", category: "Action", id: "7400daf59c4fb83b7aa1a26a5d65a654")]
public partial class SetMovePosInArea : Action
{
    [SerializeReference] public BlackboardVariable<Vector3> MovePos;
    [SerializeReference] public BlackboardVariable<WanderArea> Area;
    [SerializeReference] public BlackboardVariable<float> PatrolRadius;
    protected override Status OnStart()
    { 
        
        if (Area.Value.TryGetRandomPointOnNavmesh(out Vector3 pos,PatrolRadius))
        {
            MovePos.Value = pos;
            return Status.Success;
        }

        Debug.LogWarning(" No valid NavMesh position found inside area.");
        return Status.Failure;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

