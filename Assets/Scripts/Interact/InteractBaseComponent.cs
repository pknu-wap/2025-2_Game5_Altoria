using GameInteract;
using System;
using UnityEngine;

namespace GameInteract
{
    public class InteractBaseComponent : BaseEntityComponent, IInteractable
    {
        public bool CanInteract { get; private set; }

        public bool IsInteract { get; private set; }

        public event Action OnInteractionEnded;

        public virtual void EnterInteract()
        {
            CanInteract = true;
            OnEndInteract();
        }
        public virtual void OnEnterInteract() { }
        public virtual void ExitInteract()
        {
            CanInteract = false;
            IsInteract = false;
        }
        public virtual void Interact()
        {
            IsInteract = true;
            OnInteract();
        }
        protected virtual void OnInteract() { }

        protected virtual void EndInteract()
        {
            OnInteractionEnded?.Invoke();
            IsInteract = false;
            OnEndInteract();
        }
        protected virtual void OnEndInteract() { }

    }

}
