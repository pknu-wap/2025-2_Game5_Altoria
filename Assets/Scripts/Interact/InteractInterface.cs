using System;
using UnityEngine;

namespace Interact
{
    public interface IInteractable
    {
        bool CanInteract { get; }
        void EnterInteract();
        void ExitInteract();
    }
    public interface WorldUIShoawable{  void ShowWorldUI(Transform actor);}
    public interface IInteractEnable { }
    public interface IInteractEnable<T> { Action<T> EndInvoke { get; } }
}