using GameInteract;
using UnityEngine;

namespace GameInteract
{
    public abstract class InteractBaseComponent : BaseEntityComponent, IInteractable
    {
        public bool CanInteract { get; private set; }
        public void EnterInteract() { CanInteract = true; }
        public void ExitInteract() { CanInteract = false; }
        public abstract void Interact();
        
    }

}
