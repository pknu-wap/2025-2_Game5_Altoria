using System;
using UnityEngine;
using Interact;

public class PlayerInteractComponent : MonoBehaviour
{
    [Header("RayCast 구 크기")]
    [Range(0, 10)]
    [SerializeField] float interactRadius = 3f;

    [SerializeField] LayerMask interactableLayer;

    [Header("상호작용 오브젝트 제한")]
    [SerializeField] int interactCount;

    Transform origin;
    InteractBaseComponent currentTarget;
    InteractBaseComponent prevTarget;

    Collider[] hitBuffer;

    private void Awake()
    {
        origin = transform;
        hitBuffer = new Collider[interactCount]; 
    }

    void Update() { CheckInteract(); }

    void CheckInteract()
    {
        currentTarget = FindClosestInteractable();

        if (currentTarget != null)
        {
            if (currentTarget != prevTarget)
            {
                prevTarget?.ExitInteract();
                prevTarget = currentTarget;
            }
            currentTarget.EnterInteract();
        }
        else if (prevTarget != null) 
        {
            prevTarget.ExitInteract();
            prevTarget = null;
        }
    }

    InteractBaseComponent FindClosestInteractable()
    {
        int hitCount = Physics.OverlapSphereNonAlloc(
            origin.position,
            interactRadius,
            hitBuffer,
            interactableLayer
        );

        float closestDistSqr = float.MaxValue;
        InteractBaseComponent closest = null;

        for (int i = 0; i < hitCount; i++)
        {
            var interactable = hitBuffer[i].GetComponent<InteractBaseComponent>();
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

    private void OnDrawGizmos()
    {
        if (origin == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(origin.position, interactRadius);

        if (currentTarget != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(origin.position, currentTarget.transform.position);
            Gizmos.DrawSphere(currentTarget.transform.position, 0.1f);
        }
    }
}
