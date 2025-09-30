// InteractionSystem.cs
using GameInteract;
using System;
using System.Diagnostics;

public enum InteractionState
{
    None,
    Entered,
    Interacting,
    Holding,
    Exited
}

public class InteractionSystem
{
    public InteractionState CurrentState { get; private set; } = InteractionState.None;
    public IInteractable CurrentTarget { get; private set; }

    public void UpdateTarget(IInteractable newTarget)
    {
        if(newTarget==null)return;      

        if (newTarget != CurrentTarget)
        {
            if (CurrentTarget != null)
            {
                CurrentTarget.ExitInteract();
                CurrentState = InteractionState.Exited;
            }

            CurrentTarget = newTarget;

            if (CurrentTarget != null)
            {
                CurrentTarget.EnterInteract();
                CurrentState = InteractionState.Entered;
            }
            else CurrentState = InteractionState.None;
        }
        else
        {
            if (CurrentTarget == null) return;
            if(!CurrentTarget.CanInteract)
            {
                CurrentTarget.EnterInteract();
                CurrentState = InteractionState.Entered;
            }
        }
       
    }

    public void TryInteract()
    {
        if (CurrentState == InteractionState.Entered && CurrentTarget != null)
        {
            UnityEngine.Debug.Log("[InteractionSystem]: Interact");
            CurrentTarget.Interact();
            CurrentState = InteractionState.Interacting;
            CurrentTarget.OnInteractionEnded += HandleInteractionEnded;
        }
    }

    public void HoldInteract()
    {
        if (CurrentState == InteractionState.Interacting && CurrentTarget is IInteractStay stay)
        {
            stay.HoldInteract();
            CurrentState = InteractionState.Holding;
        }
    }
    void HandleInteractionEnded()
    {
        CurrentTarget.OnInteractionEnded -= HandleInteractionEnded;
        EndInteract();
    }

    public void EndInteract()
    {
        if (CurrentTarget != null) CurrentTarget.ExitInteract();
        CurrentState = InteractionState.None;
    }
}
