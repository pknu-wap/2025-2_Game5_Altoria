using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(GroundChecker))]
[RequireComponent(typeof(NavMeshAgent))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] float speed = 5f;
    [SerializeField] float jumpHeight = 3f;
    [SerializeField] float gravity = -9.81f * 2f;
    [SerializeField] Transform modelTransform;

    CharacterController controller;
    GroundChecker groundChecker;
    NavMeshAgent agent;
    Transform mainCamera;

    Vector3 velocity;
    Vector3 moveInput;
    Vector3 navMove;
    bool useNavPath;

    public bool IsGrounded => groundChecker.IsGrounded;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        groundChecker = GetComponent<GroundChecker>();
        agent = GetComponent<NavMeshAgent>();

        agent.updatePosition = false;
        agent.updateRotation = false;

        if (Camera.main != null)
            mainCamera = Camera.main.transform;
    }

    void FixedUpdate() => HandleMovement();

    void HandleMovement()
    {
        bool isGrounded = groundChecker.CheckGrounded();
        ApplyGravity(isGrounded);

        if (useNavPath) UpdateNavMovement();
        else MoveCharacter();

        RotateModel();
        agent.nextPosition = transform.position;
    }

    void ApplyGravity(bool isGrounded)
    {
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        velocity.y += gravity * Time.fixedDeltaTime;
        controller.Move(velocity * Time.fixedDeltaTime);
    }

    void MoveCharacter()
    {
        if (mainCamera == null) return;

        Vector3 camForward = mainCamera.forward;
        Vector3 camRight = mainCamera.right;
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 move = camForward * moveInput.z + camRight * moveInput.x;
        controller.Move(move * speed * Time.fixedDeltaTime);
    }

    void UpdateNavMovement()
    {
        if (!agent.hasPath || agent.pathPending)
            return;

        Vector3 target = agent.steeringTarget;
        navMove = target - transform.position;
        navMove.y = 0f;

        if (navMove.sqrMagnitude < 0.01f)
        {
            useNavPath = false;
            return;
        }

        navMove.Normalize();
        controller.Move(navMove * speed * Time.fixedDeltaTime);
    }

    void RotateModel()
    {
        Vector3 move = useNavPath ? navMove : controller.velocity;
        move.y = 0;

        if (move.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            modelTransform.rotation = Quaternion.Slerp(modelTransform.rotation, targetRotation, Time.fixedDeltaTime * 10f);
        }
    }


    public void SetMoveInput(Vector3 input)
    {
        moveInput = input;
        if (input.sqrMagnitude > 0.01f)
            useNavPath = false;
    }

    public void SetDestination(Vector3 destination)
    {
        if (!agent.isOnNavMesh)
        {
            Debug.LogWarning("NavMeshAgent is not on a NavMesh!");
            return;
        }
        agent.SetDestination(destination);
        useNavPath = true;
    }

    public void Jump()
    {
        if (groundChecker.IsGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            useNavPath = false;
        }
    }

    public void Stop()
    {
        moveInput = Vector3.zero;
        useNavPath = false;
        agent.ResetPath();
    }

    public Transform GetTransform() => transform;
}
