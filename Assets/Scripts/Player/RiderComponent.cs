using System;
using UnityEngine;
using GameInteract;

[DisallowMultipleComponent]
public class RiderComponent : MonoBehaviour
{
    [Header("탑승 감지 설정")]
    [Range(1f, 10f)][SerializeField] private float interactRange = 3f;
    [SerializeField] private LayerMask ridingMask = ~0;
    [SerializeField] private bool drawGizmo = true;

    private bool isRiding;
    private IRiding currentMount;
    private Camera mainCam;

    public event Action<bool, IRiding> OnRideChanged;

#if UNITY_EDITOR
    private readonly Collider[] hitBuffer = new Collider[10];
#endif

    public IRiding CurrentMount => currentMount;
    public bool IsRiding => isRiding;

    void Awake()
    {
        mainCam = Camera.main;
    }

    void Update()
    {
        DetectClosestMount();
    }


    void DetectClosestMount()
    {
        int hitCount = Physics.OverlapSphereNonAlloc(
            transform.position,
            interactRange,
            hitBuffer,
            ridingMask
        );

        float closestDistSqr = float.MaxValue;
        IRiding closest = null;

        for (int i = 0; i < hitCount; i++)
        {
            var hit = hitBuffer[i];
            if (hit == null) continue;

            if (hit.TryGetComponent<IRiding>(out var mount))
            {
                float distSqr = (transform.position - hit.transform.position).sqrMagnitude;
                if (distSqr < closestDistSqr)
                {
                    closestDistSqr = distSqr;
                    closest = mount;
                }
            }
        }

        if (currentMount != closest)
        {
            currentMount = closest;
            if (currentMount != null)
                Debug.Log($"[Rider] 가장 가까운 말 감지됨: {currentMount}");
            else
                Debug.Log("[Rider] 감지 해제됨");
        }
    }


    public void TryRide(IEntity player)
    {
        if (currentMount == null)
        {
            Debug.Log("[Rider] 탑승 가능한 대상 없음");
            return;
        }

        if(!isRiding)
        {
            currentMount.OnMounted += HandleMounted;
            currentMount.OnDismounted += HandleDismounted;
        }

        currentMount.Ride(player);
    }

    
    public void HandleMounted(IEntity entity)
    {
        isRiding = true;
       
        OnRideChanged?.Invoke(true, currentMount);
        Debug.Log($"[Rider] Mounted: {currentMount}");
    }

    public void HandleDismounted(IEntity entity)
    {
        isRiding = false;
        currentMount = null;
        OnRideChanged?.Invoke(false, currentMount);
        Debug.Log($"[Rider] Dismounted: {currentMount}");
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if (!drawGizmo) return;

        Vector3 origin = transform.position;

  
        Gizmos.color = new Color(0f, 0.8f, 1f, 0.3f);
        Gizmos.DrawWireSphere(origin, interactRange);

        if (!Application.isPlaying) return;


        int hitCount = Physics.OverlapSphereNonAlloc(origin, interactRange, hitBuffer, ridingMask);
        Gizmos.color = new Color(1f, 0.6f, 0.1f, 0.4f);

        for (int i = 0; i < hitCount; i++)
        {
            if (hitBuffer[i] == null) continue;
            var t = hitBuffer[i].transform;
            Gizmos.DrawLine(origin, t.position);
            Gizmos.DrawSphere(t.position, 0.08f);
        }


        if (currentMount is MonoBehaviour mb)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(origin, mb.transform.position);
            Gizmos.DrawSphere(mb.transform.position, 0.15f);
        }
    }
#endif
}
