using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    [Header("Ground Check Settings")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Vector3 centerOffset = Vector3.zero;
    [SerializeField] private float groundDistance = 0.3f;
    [SerializeField] private LayerMask groundMask;

    public bool IsGrounded { get; private set; }

    public bool CheckGrounded()
    {
        if (groundCheck == null)
        {
            Debug.LogWarning($"{name}: GroundCheck Transform not assigned!");
            return false;
        }


        Vector3 checkPos = groundCheck.position + centerOffset;

        IsGrounded = Physics.CheckSphere(checkPos, groundDistance, groundMask);
        return IsGrounded;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
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
