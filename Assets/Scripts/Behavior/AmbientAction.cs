using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "AmbientAction", story: "[Self] do Ambient", category: "Action", id: "868e027e925d45c8d15c1e90520e8bde")]
public partial class AmbientAction : Action
{
    [SerializeReference] public BlackboardVariable<Transform> Self;

    protected override Status OnStart()
    {
        if (!Self.Value.TryGetComponent<IAmbient>(out var ambient)) return Status.Failure;
        
        ambient.Ambient();

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

