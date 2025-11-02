using UnityEngine;
using UnityEngine.AI;

public class NavMeshLoader : MonoBehaviour
{
    [SerializeField] private NavMeshData navMeshData;
    private NavMeshDataInstance currentInstance;

    void Start()
    {
        
        if (navMeshData != null)
            currentInstance = NavMesh.AddNavMeshData(navMeshData);
    }

    void OnDestroy()
    {
     
        currentInstance.Remove();
    }
}
