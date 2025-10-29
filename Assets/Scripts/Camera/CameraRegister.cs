using Unity.Cinemachine;
using UnityEngine;

public class CameraRegister : MonoBehaviour
{
    CinemachineCamera cam;
    string id;

    void Awake()
    {
        cam = GetComponent<CinemachineCamera>();

        id = !string.IsNullOrEmpty(gameObject.name)
            ? gameObject.name
            : $"Camera_{cam.GetInstanceID()}";
    }

    void OnEnable()
    {
        CameraHub.Instance?.RegisterCamera(id, cam);
    }

    void OnDisable()
    {
        CameraHub.Instance?.UnregisterCamera(id);
    }

    public void SetActiveCamera()
    {
        CameraHub.Instance?.SetActiveCamera(id);
    }

    public string GetID() => id;
}