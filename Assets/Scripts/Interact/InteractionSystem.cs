// InteractionSystem.cs
using GameInteract;
using System;
using System.Diagnostics;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

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

    public Action<int> InteractInvoke;
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

    public void TryInteract(IEntity entity)
    {
        if (CurrentState == InteractionState.Entered && CurrentTarget != null)
        {
            RotateEntityToTarget(entity, CurrentTarget);
            UnityEngine.Debug.Log("[InteractionSystem]: Interact");
            InteractInvoke.Invoke((int)(CurrentTarget.Type)); 
            CurrentTarget.Interact(entity);
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
        InteractInvoke.Invoke(0);
    }
    void RotateEntityToTarget(IEntity entity, IInteractable target)
    {
        Transform rotateTarget = (entity is IModel model) ? model.Model : entity.transform;

        Vector3 targetPos = Vector3.zero;
        if (target is IEntity targetEntity) targetPos = targetEntity.transform.position;


        Vector3 dir = targetPos - rotateTarget.position;
        dir.y = 0; 

        if (dir.sqrMagnitude > 0.001f)
            rotateTarget.rotation = Quaternion.LookRotation(dir);
    }

}
