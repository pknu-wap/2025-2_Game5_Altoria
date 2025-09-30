using System;
using UnityEngine;

namespace GameInteract
{
   
    public class CraftingTimer : ITimer
    {
        public float Elapsed { get; private set; }
        public float Duration { get; private set; }
        public bool IsFinished => Elapsed <= Duration;

        public event Action<ITimer> OnFinished;

        public void Reset()
        {
            
        }

        public void SetTimer(float duration, bool autoRegister = true)
        {
            Duration = duration;
            Elapsed = 0f;

            if (autoRegister)
                Manager.Time.RegisterTimer(this);
        }

        public void Tick(float deltaTime)
        {
            throw new NotImplementedException();
        }
    }
}