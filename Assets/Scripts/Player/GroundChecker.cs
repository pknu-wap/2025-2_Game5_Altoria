using UnityEngine;
using System;

[DisallowMultipleComponent]
public class GroundChecker : MonoBehaviour
{
    [Header("Ground Check Settings")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Vector3 centerOffset = Vector3.zero;
    [SerializeField, Range(0.05f, 1f)] private float groundDistance = 0.3f;
    [SerializeField] private LayerMask groundMask;

    public bool IsGrounded { get; private set; }

    public event Action<bool> OnGroundedChanged;

     bool lastGrounded;

    void Awake()
    {
        if (groundCheck == null)
            groundCheck = transform;
    }

     void Update()
    {
        CheckGrounded();
    }

    public bool CheckGrounded()
    {
        if (groundCheck == null)
            return false;

        Vector3 checkPos = groundCheck.position + centerOffset;

        bool groundedNow = Physics.CheckSphere(checkPos, groundDistance, groundMask);


        if (groundedNow != lastGrounded)
        {
            OnGroundedChanged?.Invoke(groundedNow);
            lastGrounded = groundedNow;
        }

        IsGrounded = groundedNow;
        return IsGrounded;
    }

#if UNITY_EDITOR
     void OnDrawGizmosSelected()
    {
        if (groundCheck == null)
            return;

        Vector3 checkPos = groundCheck.position + centerOffset;
        Gizmos.color = IsGrounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(checkPos, groundDistance);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(groundCheck.position, checkPos);
    }
#endif
}
