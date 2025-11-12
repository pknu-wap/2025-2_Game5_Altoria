using System.Collections;
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

    private void OnEnable()
    {
        StartCoroutine(RegisterWhenReady());
    }

    IEnumerator RegisterWhenReady()
    {
        
        yield return new WaitUntil(() => CameraHub.Instance != null);
        CameraHub.Instance.RegisterCamera(id, cam);
        Debug.Log($"[CameraRegister] 카메라 등록 완료 → {id}");
    }

     void OnDisable()
    {
        if (CameraHub.Instance != null)
        {
            CameraHub.Instance.UnregisterCamera(id);
            Debug.Log($"[CameraRegister] 카메라 해제 → {id}");
        }
    }

    public void SetActiveCamera()
    {
        CameraHub.Instance?.SetActiveCamera(id);
    }

    public string GetID() => id;
}