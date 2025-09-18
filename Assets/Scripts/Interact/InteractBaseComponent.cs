using Interact;
using UnityEngine;

namespace Interact
{
    public abstract class InteractBaseComponent : BaseEntityComponent, IInteractable
    {
        public bool CanInteract { get; private set; }
        public void EnterInteract() { CanInteract = true; }
        public void ExitInteract() { CanInteract = false; }
        public abstract void Interact();

    }

}
