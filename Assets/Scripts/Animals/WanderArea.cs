using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(BoxCollider))]
public class WanderArea : MonoBehaviour
{
    BoxCollider box;

    void Awake() => box = GetComponent<BoxCollider>();

    public bool TryGetRandomPointOnNavmesh(out Vector3 result, float sampleRadius = 2f)
    {
       
        Vector3 extents = box.size * 0.5f;
        Vector3 randomLocal = new Vector3(
            Random.Range(-extents.x, extents.x),
            Random.Range(-extents.y, extents.y),
            Random.Range(-extents.z, extents.z)
        );

        Vector3 worldPoint = box.transform.TransformPoint(box.center + randomLocal);

 
        if (NavMesh.SamplePosition(worldPoint, out NavMeshHit hit, sampleRadius, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0f, 1f, 0f, 0.25f);
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawCube(Vector3.zero, GetComponent<BoxCollider>().size);
    }
#endif
}
