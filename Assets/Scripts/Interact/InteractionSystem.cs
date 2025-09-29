// InteractionSystem.cs
using GameInteract;
using System;

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
            else
            {
                CurrentState = InteractionState.None;
            }
        }
    }

    public void TryInteract()
    {
        if (CurrentState == InteractionState.Entered && CurrentTarget != null)
        {
            CurrentTarget.Interact();
            CurrentState = InteractionState.Interacting;
            void HandleInteractionEnded()
            {
                CurrentTarget.OnInteractionEnded -= HandleInteractionEnded;
                EndInteract();
            }

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
        if (CurrentTarget != null)
        {
            CurrentTarget.ExitInteract();
            CurrentTarget = null;
        }
        CurrentState = InteractionState.None;
    }
}
