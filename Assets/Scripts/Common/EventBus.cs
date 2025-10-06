using System;
using System.Collections.Generic;

public static class GlobalEvents { public static readonly EventBus Instance = new(); }
public class EventBus
{
    readonly Dictionary<Type, List<Delegate>> subScribers = new();

    class Subscription<T> : IDisposable
    { 
        readonly EventBus bus;
        readonly Action<T> callback;
        bool disposed;

        public Subscription(EventBus bus, Action<T> callback)
        {
            this.bus = bus;
            this.callback = callback;
        }

        public void Dispose()
        {
            if (disposed) return;
            disposed = true;
            bus.Unsubscribe(callback);
        }
    }
    public IDisposable Subscribe<T>(Action<T> callback)
    {
        var type = typeof(T);
        if (!subScribers.ContainsKey(type))
        {
            subScribers[type] = new List<Delegate>();
        }
        subScribers[type].Add(callback);
        return new Subscription<T>(this, callback);
    }

    public void Unsubscribe<T>(Action<T> callback)
    {
        var type = typeof(T);
        if (subScribers.TryGetValue(type, out var list))
        {
            list.Remove(callback);
        }
    }

   
    public void Publish<T>(T evt)
    {
        var type = typeof(T);
        if (subScribers.TryGetValue(type, out var list))
        {
            for (int i = 0; i < list.Count; i++)
            {
                (list[i] as Action<T>)?.Invoke(evt);
            }
        }
    }

    public void Clear()
    {
        subScribers.Clear();
    }
}

