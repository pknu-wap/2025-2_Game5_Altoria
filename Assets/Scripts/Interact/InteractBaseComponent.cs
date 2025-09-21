using GameInteract;
using UnityEngine;

namespace GameInteract
{
    public abstract class InteractBaseComponent : BaseEntityComponent, IInteractable,IInteractDelay
    {
        public bool CanInteract { get; private set; }
        public float Delay => throw new System.NotImplementedException();

        public void EnterInteract() { CanInteract = true; }
        public void ExitInteract() { CanInteract = false; }
        public abstract void Interact();
        
    }

}
