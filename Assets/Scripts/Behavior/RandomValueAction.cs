using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "RandomValue", story: "Get Random [Weight]", category: "Action", id: "2868be6dec7e6c6f488b603d38c6474d")]
public partial class RandomValueAction : Action
{
    [SerializeReference] public BlackboardVariable<float> Weight;

    protected override Status OnStart()
    {
        float rand = UnityEngine.Random.Range(0.0f, 1.0f);
        Weight.Value = rand;
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

