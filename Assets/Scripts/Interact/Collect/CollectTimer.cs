using System;
using UnityEngine;

namespace GameInteract
{
    // 채집 컴포넌트에서 상호작용이 시작되면 타이머를 동작한다.
    public class CollectTimer : ITimer
    {
        public float Elapsed { get; private set; }
        public float Duration { get; private set; }
        public bool IsFinished => Duration <= Elapsed;

        public event Action<ITimer> OnFinished;

        public void Reset() => Elapsed = 0f;

        public CollectTimer(float duration)
        {
            Duration = duration;
            Elapsed = 0f;

            Manager.Time.RegisterTimer(this);
        }


        public void Tick(float deltaTime)
        {
            if (IsFinished) return;
            Elapsed += deltaTime;

            if (IsFinished) OnFinished?.Invoke(this);
        }
    }

}