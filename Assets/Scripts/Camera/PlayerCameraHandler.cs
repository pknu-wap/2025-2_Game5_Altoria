using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraHandler : MonoBehaviour
{
    [Header("플레이어 카메라 리스트(기본제외)")]
    [SerializeField] List<CameraRegister> playerCameras = new();

    [Header("기본 카메라 이름")]
    [SerializeField]  CameraRegister defaultCamera;

    CameraRegister currentCamera;

    void Start()
    {
        SetCamera(defaultCamera.gameObject.name);
    }


    public void SetCamera(string cameraName)
    {
        if (string.IsNullOrEmpty(cameraName))
        {
            Debug.LogWarning("[PlayerCameraHandler] 잘못된 카메라 이름입니다.");
            return;
        }

        var found = playerCameras.Find(c => c != null && c.GetID() == cameraName);
        if (found == null)
        {
            Debug.LogWarning($"[PlayerCameraHandler] '{cameraName}' 카메라를 찾을 수 없습니다.");
            return;
        }

        currentCamera = found;
        currentCamera.SetActiveCamera();
        Debug.Log($"[PlayerCameraHandler] Active Camera → {cameraName}");
    }


    public void SetDefaultCamera()
    {
        SetCamera(defaultCamera.gameObject.name);
    }


    public CameraRegister GetCurrentCamera() => currentCamera;

#if UNITY_EDITOR
    [Header("디버그 설정")]
    [SerializeField] bool drawGizmos = true;

    void OnDrawGizmosSelected()
    {
        if (!drawGizmos || playerCameras == null) return;

        Gizmos.color = Color.cyan;
        for(int i=0; i<playerCameras.Count; i++)
        {
            if (playerCameras[i] == null) continue;
            Gizmos.DrawLine(transform.position, playerCameras[i].transform.position);
            Gizmos.DrawSphere(playerCameras[i].transform.position, 0.1f);
        }
    }
      
#endif
    
}
