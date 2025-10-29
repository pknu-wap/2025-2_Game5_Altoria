using UnityEngine;

public class Billboard : MonoBehaviour 
{
    void Update()
    {
        transform.forward = Camera.main.transform.forward;
    }
}
