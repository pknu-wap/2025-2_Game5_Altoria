using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class CameraHub : MonoBehaviour
{
    public static CameraHub Instance { get; private set; }

    CinemachineCamera activeCamera;
    readonly Dictionary<string, CinemachineCamera> cameras = new();

    [SerializeField] int activePriority = 20;
    [SerializeField] int inactivePriority = 0;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Debug.Log("[CameraHub]: Awake");
        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Lobby")
            Destroy(gameObject);
    }

    public void RegisterCamera(string id, CinemachineCamera cam)
    {
        if (string.IsNullOrEmpty(id) || cam == null)
         {
            Debug.Log($"{id}: {cam}");
            return;
        }
        Debug.Log($"[CameraHub]: 등록 {id}");
        if (cameras.ContainsKey(id))
            Debug.LogWarning($"[CameraHub] 이미 등록된 카메라 ID '{id}'가 있습니다. 기존 항목을 덮어씁니다.");

        cameras[id] = cam;
    }

    public void UnregisterCamera(string id)
    {
        if (string.IsNullOrEmpty(id))
            return;

        cameras.Remove(id);
    }

    public void SetActiveCamera(string id)
    {
        if (!cameras.TryGetValue(id, out var targetCam)) return;
       

        if (activeCamera == targetCam)
            return;

        foreach (var cam in cameras.Values)
            cam.Priority = inactivePriority;

        targetCam.Priority = activePriority;
        activeCamera = targetCam;
    }

    public CinemachineCamera GetActiveCamera() => activeCamera;
    public CinemachineCamera GetCamera(string id) => cameras.TryGetValue(id, out var cam) ? cam : null;
    public bool HasCamera(string id) => cameras.ContainsKey(id);
}
