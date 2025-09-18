using UnityEngine;

namespace Interact
{
    public interface IInteractable
    {
        bool CanInteract { get; }
        void EnterInteract();
        void ExitInteract();
    }

}