using UnityEngine;

public class TimeControllerComponent : MonoBehaviour
{
    void Update()
    {
        TimeController.Instance.Tick(Time.deltaTime);
    }
}
