using System;
using UnityEngine;
using static Define;

[RequireComponent(typeof(PlayerInputHandler))]
[RequireComponent(typeof(PlayerMovement))]
public class PlayerController : BaseEntityComponent
{
    [SerializeField] Animator animator;

    PlayerInputHandler input;
    PlayerMovement movement;
    PlayerInteractComponent interact;
    GroundChecker groundChecker;

    public PlayerStateController State { get; private set; } = new PlayerStateController();

    bool isJump = false;

    void Awake()
    {
        input = GetComponent<PlayerInputHandler>();
        movement = GetComponent<PlayerMovement>();
        interact = GetComponent<PlayerInteractComponent>();
        groundChecker = GetComponent<GroundChecker>();

       
    }

    void OnEnable()
    {
        groundChecker.OnGroundedChanged += OnGround;
        State.OnStateChanged += OnStateChanged;
        interact.InteractSystem.InteractInvoke += OnInteract;
        BindInputEvents();
    }

    private void OnDisable()
    {
        groundChecker.OnGroundedChanged -= OnGround;
        State.OnStateChanged -= OnStateChanged;
        interact.InteractSystem.InteractInvoke -= OnInteract;
        UnbindInputEvents();
    }

   
    void BindInputEvents()
    {
        input.OnMove += OnMove;
        input.OnMoveCanceled += OnMoveCanceled;
        input.OnInteract += TryInteract;
        input.OnJump += OnJump;
    }

    void UnbindInputEvents()
    {
        input.OnMove -= OnMove;
        input.OnMoveCanceled -= OnMoveCanceled;
        input.OnJump -= OnJump;
        input.OnInteract -= TryInteract;
    }


   void OnMove(Vector2 inputDir)
    {
        if (!State.CanReceiveInput()) return;

        movement.SetMoveInput(new Vector3(inputDir.x, 0f, inputDir.y));
        animator.SetBool("IsMove", true);
        State.SetState(PlayerState.Move);
    }

    void OnMoveCanceled()
    {

        animator.SetBool("IsMove", false);
        movement.SetMoveInput(Vector3.zero);
        State.SetState(PlayerState.Idle);
    }
    void OnGround(bool grounded)
    {
        if (grounded)
        {
            isJump = false;
            if (!State.Is(PlayerState.Move))
                State.SetState(PlayerState.Idle);
        }

        animator.SetBool("IsGround", grounded);
    }
    void OnJump()
    {
        if (!State.CanReceiveInput() || !movement.IsGrounded || isJump)
            return;

        isJump = true;
        animator.SetTrigger("IsJump");
        movement.Jump();
        State.SetState(PlayerState.Jump);
    }

    void TryInteract()
    {
        if (State.CurrentState==PlayerState.Die) return;


        interact.TryInteract();
        State.SetState(PlayerState.Interacting);
    }
    void OnInteract(int type)
    {
        if (State.CurrentState == PlayerState.Die)
            return;

        if (animator.GetInteger("InteractType") != type)
            animator.SetInteger("InteractType", type);

        if (type == 0)
        {
            animator.SetBool("IsInteract", false);
            State.SetState(PlayerState.Idle);
        }
        else
            animator.SetBool("IsInteract", true);
    }

    public void OnDie()
    {
        input.enabled = false;
        movement.SetMoveInput(Vector3.zero);
        animator.SetTrigger("Die");
        State.SetState(PlayerState.Die);
    }

    public void OnRevive()
    {
        input.enabled = true;
        State.SetState(PlayerState.Idle);
    }
    

    void OnStateChanged(PlayerState newState)
    {
        Debug.Log($"[PlayerController] State changed ¡æ {newState}");
    }
}
