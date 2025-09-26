using GameInteract;
using System;
using System.Collections.Generic;
using UnityEngine;

public enum TimeMode
{
    Morning,
    Night
}

public class TimeController
{
    List<ITimer> timers = new List<ITimer>();

    public TimeController() { }
    

    public void Tick(float deltaTime)
    {
        for (int i = timers.Count - 1; i >= 0; i--)
        {
            timers[i].Tick(deltaTime);
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
