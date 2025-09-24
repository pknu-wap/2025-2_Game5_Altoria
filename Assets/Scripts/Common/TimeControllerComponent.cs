using UnityEngine;

public class TimeControllerComponent : MonoBehaviour
{
    TimeController time;
    void Awake() { this.time = TimeController.Instance; }
    void Update() { this.time.Tick(Time.deltaTime); }
}
