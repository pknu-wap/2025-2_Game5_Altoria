using GameInteract;
using System;
using System.Collections.Generic;
using UnityEngine;

public enum TimeMode
{
    Morning,
    Night
}

public class TimeController : MonoBehaviour
{
     static TimeController instance;    
     public static TimeController Instance
    {
        get
        {
            if (instance == null)
            {
                var go = new GameObject(nameof(TimeController));
                instance = go.AddComponent<TimeController>();
                DontDestroyOnLoad(go);
            }
            return instance;
        }
    }

    List<ITimer> timers = new List<ITimer>();

    void Update()
    {
        Tick();
    }

    void Tick()
    {
        for (int i = timers.Count - 1; i >= 0; i--)
        {
            timers[i].Tick(Time.deltaTime);
        }
    }

    public void RegisterTimer(ITimer timer)
    {
        timers.Add(timer);
        timer.OnFinished += RemoveTimer;
    }

    void RemoveTimer(ITimer timer)
    {
        timer.OnFinished -= RemoveTimer;
        timers.Remove(timer);
    }
}
