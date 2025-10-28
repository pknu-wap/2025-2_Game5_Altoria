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
    [SerializeField] private float groundedBufferTime = 0.1f;
   
    float groundedTimer;
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
        bool hit = Physics.CheckSphere(checkPos, groundDistance, groundMask);

        if (hit != lastGrounded)
        {
            groundedTimer += Time.deltaTime;
            if (groundedTimer >= groundedBufferTime)
            {
                OnGroundedChanged?.Invoke(hit);
                lastGrounded = hit;
                groundedTimer = 0f;
            }
        }
        else
        {
            groundedTimer = 0f;
        }

        IsGrounded = lastGrounded;
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
