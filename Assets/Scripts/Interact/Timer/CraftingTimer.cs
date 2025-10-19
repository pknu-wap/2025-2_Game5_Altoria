using Common;
using System;
using UnityEditor;
using UnityEngine;

namespace GameInteract
{
   
    public class CraftingTimer : ITimer,IProgress<float>
    {
        public event Action<float> OnProgress;

        public event Action<ITimer> OnFinished;
        public bool IsRunning { get; private set; }
        public float Elapsed {  get; private set; } 
        public float Duration { get; private set; }
        public bool IsFinished => (Elapsed <= Duration);
     
        public void Stop() => IsRunning = false;

        public void SetTimer(float duration, bool autoRegister = true)
        {
            Duration = duration;
            Elapsed = 0f;
            IsRunning = true;

            if (autoRegister)
                GameSystem.Time.RegisterTimer(this);
        }

        void ITimer.Tick(float deltaTime)
        {
            if (!IsRunning) return;

            Elapsed += deltaTime;

            float progress = Mathf.Clamp01(Elapsed / Duration);

            OnProgress?.Invoke(progress);

            if (Elapsed >= Duration)
                Reset();
        }

        public void Reset()
        {
            IsRunning = false;
            OnFinished?.Invoke(this);
        }
    }
}