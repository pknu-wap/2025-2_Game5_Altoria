using UnityEngine;

public class MiniMapMarker : MonoBehaviour
{
    Transform miniMapCamera;

    void Start()
    {
        Transform camObj = GameObject.FindGameObjectWithTag("MiniMapCamera").transform;

        if (camObj != null) miniMapCamera = camObj;
    }


    void LateUpdate() => RotateWithMiniMapCamera();

    public void RotateWithMiniMapCamera()
    {
        if (miniMapCamera == null) return;

        Vector3 rot = transform.eulerAngles;
        rot.y = miniMapCamera.eulerAngles.y;
        transform.eulerAngles = rot;
    }
}
