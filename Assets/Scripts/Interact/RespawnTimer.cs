using System;
using System.Diagnostics;

namespace GameInteract
{
    public class RespawnTimer : ITimer
    {
        public float Elapsed { get; private set; }
        public float Duration { get; }
        public bool IsFinished => Elapsed >= Duration;

        public event Action<ITimer> OnFinished;

        public RespawnTimer(float duration, bool autoRegister = true)
        {
            Duration = duration;
            Elapsed = 0f;

            if (autoRegister)
                TimeController.Instance.RegisterTimer(this);
        }

        public void Tick(float deltaTime)
        {
            if (IsFinished) return;
            Elapsed += deltaTime;

            UnityEngine.Debug.Log(Elapsed);

            if (IsFinished) OnFinished?.Invoke(this);
        }

        public void Reset() => Elapsed = 0f;
    }
}
