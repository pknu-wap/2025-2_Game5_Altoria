
using System;
using UnityEngine;
using GameInteract;
using UnityEngine.InputSystem;

public class PlayerInteractComponent : MonoBehaviour
{
    [Header("RayCast 구 크기")]
    [Range(0, 10)]
    [SerializeField] float interactRadius = 3f;

    [SerializeField] LayerMask interactableLayer;
    [Header("상호작용 오브젝트 제한")]
    [SerializeField] int interactCount = 5;

    Transform origin;
    InteractionSystem interactSystem = new();
    Collider[] hitBuffer;
    IInteractable currentTarget;

    public InteractionSystem InteractSystem => interactSystem;
    public IInteractable CurrentTarget => currentTarget;
    void Awake()
    {
        origin = transform;
        hitBuffer = new Collider[interactCount];
    }

    void Update()
    {

        currentTarget = FindClosestInteractable();
        interactSystem.UpdateTarget(currentTarget);
    }

    IInteractable FindClosestInteractable()
    {
        int hitCount = Physics.OverlapSphereNonAlloc(
            origin.position,
            interactRadius,
            hitBuffer,
            interactableLayer
        );

        float closestDistSqr = float.MaxValue;
        IInteractable closest = null;

        for (int i = 0; i < hitCount; i++)
        {
            var interactable = hitBuffer[i].GetComponent<IInteractable>();
            if (interactable != null)
            {
                float distSqr = (origin.position - hitBuffer[i].transform.position).sqrMagnitude;
                if (distSqr < closestDistSqr)
                {
                    closest = interactable;
                    closestDistSqr = distSqr;
                }
            }
        }

        return closest;
    }

    public void TryInteract() => interactSystem.TryInteract();

#if UNITY_EDITOR
    [SerializeField] bool drawGizmo = true;
    private void OnDrawGizmos()
    {
        if (!drawGizmo) return;

        Vector3 originPos = transform.position;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(originPos, interactRadius);

        if (Application.isPlaying && currentTarget is MonoBehaviour mb)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(originPos, mb.transform.position);
            Gizmos.DrawSphere(mb.transform.position, 0.1f);
        }
        else
        {
            Collider[] buffer = new Collider[Math.Max(1, interactCount)];
            int hits = Physics.OverlapSphereNonAlloc(originPos, interactRadius, buffer, interactableLayer);
            Gizmos.color = new Color(1f, 0.5f, 0f, 0.5f);
            for (int i = 0; i < hits; i++)
            {
                Transform t = buffer[i].transform;
                Gizmos.DrawLine(originPos, t.position);
                Gizmos.DrawSphere(t.position, 0.05f);
            }
        }
    }
#endif

}
