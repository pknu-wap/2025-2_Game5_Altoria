using UnityEngine;

public class TimeControllerComponent : MonoBehaviour
{
    TimeController time = new();

    private void Update()
    {
        time.Tick(Time.deltaTime);
    }
    
}
