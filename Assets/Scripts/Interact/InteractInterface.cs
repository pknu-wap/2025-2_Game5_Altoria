using System;
using UnityEngine;


namespace GameInteract
{
    public interface IInteractable
    {
        bool CanInteract { get; }
        void EnterInteract();
        void ExitInteract();
        void Interact();

        event Action OnInteractionEnded;
    }
  
    public  interface IInteractStay { void HoldInteract(); }
    public interface IInteractDelay { float Delay { get; } }
    public interface WorldUIShoawable{  void ShowWorldUI(IEntity entity);}
    public interface IInteractEnable { }
    public interface IInteractEnable<T> { Action<T> EndInvoke { get; } }
    public interface IInteractSpawnable { }

    public interface IProgress { }
    public interface IProgress<T>:IProgress { event Action<T> OnProgress; }
    public interface ITimer
    {
        event Action<ITimer> OnFinished;
        void SetTimer(float duration, bool autoRegister = true);
        float Elapsed { get; }
        float Duration { get; }
        bool IsFinished { get; }
        void Tick(float deltaTime);
        void Reset();
    }
}