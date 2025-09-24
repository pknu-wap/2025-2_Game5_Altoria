using System;
using UnityEngine;
using GameInteract;
using UnityEngine.InputSystem;

public class PlayerInteractComponent : MonoBehaviour
{
    [Header("RayCast 구 크기")]
    [Range(0, 10)]
    [SerializeField] float interactRadius;

    [SerializeField] LayerMask interactableLayer;

    [Header("상호작용 오브젝트 제한")]
    [SerializeField] int interactCount;

    Transform origin;
    IInteractable currentTarget;
    IInteractable prevTarget;

    Collider[] hitBuffer;

    void Awake()
    {
        origin = transform;
        hitBuffer = new Collider[interactCount];
    }

    void Update()
    {
        CheckInteract();
        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            Debug.Log("E 키 눌림 → Interact 실행");
            TryInteract();
        }
    }
    public void TryInteract() { currentTarget?.Interact(); }
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

    private void OnDrawGizmos()
    {
        if (origin == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(origin.position, interactRadius);

        if (currentTarget is MonoBehaviour mb && mb != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(origin.position, mb.transform.position);
            Gizmos.DrawSphere(mb.transform.position, 0.1f);
        }
    }

}
