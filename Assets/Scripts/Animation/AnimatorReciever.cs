using UnityEngine;
using UnityEngine.Events;

public class AnimatorReceiver : MonoBehaviour
{
    [System.Serializable]
    public class IntEvent : UnityEvent<int> { }

    [Header("Animation Events")]
    public UnityEvent OnGenericEvent;      
    public IntEvent OnGenericIntEvent;      
    public UnityEvent OnDespawn;            

   
    public void InvokeEvent()
    {
        OnGenericEvent?.Invoke();
    }
    public void InvokeIntEvent(int value)
    {
    
        OnGenericIntEvent?.Invoke(value);
    }

    public void InvokeRecall()
    {
        OnDespawn?.Invoke();
    }
}
