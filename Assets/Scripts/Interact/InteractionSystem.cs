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

    public void ForceExit()
    {
        if (CurrentTarget != null)
        {
            CurrentTarget.ExitInteract();
            CurrentTarget = null;
        }
        CurrentState = InteractionState.None;
    }
}
