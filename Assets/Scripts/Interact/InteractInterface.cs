using System;
using UnityEngine;


namespace GmaeInteract
{
    public interface IInteractable
    {
        bool CanInteract { get; }
        void EnterInteract();
        void ExitInteract();
    }
   
    public  interface IInteractStay { void StayInteract(); }
    public interface IInteractDelay { float Delay { get; } }
    public interface WorldUIShoawable{  void ShowWorldUI(Transform actor);}
    public interface IInteractEnable { }
    public interface IInteractEnable<T> { Action<T> EndInvoke { get; } }

    public interface ITimer
    {
        Action<ITimer> TimeEndInvoke{ get; }
        float Timer { get; }

        void StartTimer();
    }

}