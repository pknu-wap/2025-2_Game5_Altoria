using System;
using UnityEngine;

public abstract class Loader
{
    public event Action<float> OnProgress;
    public event Action OnFailure;
    public event Action OnSuccess;
    public string Locator { get; protected set; }

    public abstract void Load();
    public abstract void UnLoad();
    protected void InvokeOnProgress(float progress) => OnProgress?.Invoke(progress);
    protected void InvokeOnFailure() => OnFailure?.Invoke();
    protected void InvokeOnSuccess() => OnSuccess?.Invoke();
}
