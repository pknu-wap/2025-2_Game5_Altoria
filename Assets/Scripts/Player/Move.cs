using System.Net.Sockets;
using UnityEngine;
using UnityEngine.AI;

public class Move : IMove
{
    Transform transform;
    Transform model;

    CharacterController controller;
    GroundChecker groundChecker;
    NavMeshAgent agent;

    Vector3 velocity;
    Vector3 moveInput;
    Vector3 navMove;

    bool useNavPath;

    IMoveData data;

    public bool IsGrounded => groundChecker != null && groundChecker.IsGrounded;
    public bool locked = false;
    public Move() { }
    public void SetEntity(IEntity entity)
    {
        transform = entity.transform;
        if (entity is IPlayerMovable movable) data = movable.MoveData;

        Debug.Log(data.Speed);

        controller = transform.GetComponent<CharacterController>();
        groundChecker = transform.GetComponent<GroundChecker>();
        agent = transform.GetComponent<NavMeshAgent>();
        
        if (agent != null)
        {
            agent.updatePosition = false;
            agent.updateRotation = false;
        }

        model = FindModelTransform(transform);
    }

    Transform FindModelTransform(Transform root)
    {
        if (root == null) return null;

        var animator = root.GetComponentInChildren<Animator>();
        if (animator) return animator.transform;

        return root;
    }
    
    public void Tick()
    {
        if (locked) return;
        bool isGrounded = groundChecker?.CheckGrounded() ?? true;
        ApplyGravity(isGrounded);
        if (isGrounded && agent != null && !agent.enabled)
        {
            agent.enabled = true;
            agent.nextPosition = transform.position;
        }

        if (useNavPath) UpdateNavMovement();
        else MoveCharacter();

        RotateModel();

        if (agent != null)
            agent.nextPosition = transform.position;
    }

    void ApplyGravity(bool isGrounded)
    {
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        velocity.y += data.Gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
    public void SetMovementLock(bool state)
    {
        locked = state;

        if (agent)
            agent.enabled = !state;

        if (controller)
            controller.enabled = !state;

        if (state)
        {
            moveInput = Vector3.zero;
            useNavPath = false;
            agent?.ResetPath();
        }
    }
    void MoveCharacter()
    {
        if (moveInput.sqrMagnitude < 0.001f) return;

        Vector3 move = moveInput.normalized * data.Speed * Time.deltaTime;
        controller.Move(move);
    }

    void UpdateNavMovement()
    {
        if (agent == null || !agent.hasPath || agent.pathPending)
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
        Vector3 move = navMove * data.Speed * Time.deltaTime;
        move.y = 0; 

        controller.Move(move);
    }
    void RotateModel()
    {
        if (model == null) return;

        Vector3 move = useNavPath ? navMove : controller.velocity;
        move.y = 0;

        if (move.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            model.rotation = Quaternion.Slerp(
                model.rotation, targetRotation, Time.deltaTime * 10f
            );
        }
    }

    public void SetMoveInput(Vector3 input)
    {
        moveInput = input;
        if (input.sqrMagnitude > 0.01f) useNavPath = false;

    }

    public void SetDestination(Vector3 destination)
    {
        if (agent == null || !agent.isOnNavMesh)
        {
            Debug.LogWarning("[Move] NavMeshAgent unavailable or not on NavMesh");
            return;
        }

        agent.SetDestination(destination);
        useNavPath = true;
    }

    public void Jump()
    {
        if (IsGrounded)
        {
            velocity.y = Mathf.Sqrt(data.JumpHeight * -2f * data.Gravity);
            useNavPath = false;
            if (agent != null) agent.enabled = false;

        }
    }

    public void Stop()
    {
        moveInput = Vector3.zero;
        useNavPath = false;
        agent?.ResetPath();
    }

    public Transform GetTransform() => transform;
}
