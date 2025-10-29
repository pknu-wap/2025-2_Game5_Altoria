using GameInteract;
using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "CheckInteractState", story: "[Self] is [Interacting]", category: "Variable Conditions", id: "db944f558cd47a0b07900e7edd4eae4d")]
public partial class CheckInteractState : Condition
{
    [SerializeReference] public BlackboardVariable<Transform> Self;
    [SerializeReference] public BlackboardVariable<InteractState> Interacting;

    public override bool IsTrue()
    {
        if (!Self.Value.TryGetComponent<IInteractable>(out var interactable))
            return false;

        switch(Interacting.Value)
        {
            case InteractState.None:
                return !interactable.IsInteract;
            case InteractState.Interacting:
                return interactable.IsInteract;
        }

        return false;
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
