using GameInteract;
using System;
using UnityEngine;

namespace GameInteract
{
    public abstract class InteractBaseComponent : BaseEntityComponent, IInteractable
    {
        public bool CanInteract { get; private set; }

        public event Action OnInteractionEnded;

        public virtual void EnterInteract() => CanInteract = true;
        public virtual void ExitInteract() => CanInteract = false;
        public abstract void Interact();
        protected virtual void EndInteract() => OnInteractionEnded?.Invoke();

    }

}
