using System;
using UnityEngine;
using static Define;

[RequireComponent(typeof(PlayerInputHandler))]
[RequireComponent(typeof(PlayerMovement))]
public class PlayerController : BaseEntityComponent
{
    [SerializeField] Animator animator;
    [SerializeField] SocketHandler socketHandler;

    PlayerInputHandler input;
    PlayerMovement movement;
    PlayerInteractComponent interact;
    GroundChecker groundChecker;
    AnimatorControllerWrapper animController;

    public PlayerStateMachine State { get; private set; } = new PlayerStateMachine();
    [SerializeField] bool isJump = false;

    void Awake()
    {
        input = GetComponent<PlayerInputHandler>();
        movement = GetComponent<PlayerMovement>();
        interact = GetComponent<PlayerInteractComponent>();
        groundChecker = GetComponent<GroundChecker>();
        animController = new AnimatorControllerWrapper(animator, this);
    }

    void OnEnable()
    {
        groundChecker.OnGroundedChanged += OnGround;
        State.OnStateChanged += OnStateChanged;
        interact.InteractSystem.InteractInvoke += OnInteract;
        BindInputEvents();
    }

    void OnDisable()
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
        animController.SetBool("IsMove", true);
        State.SetState(PlayerState.Move);
    }

    void OnMoveCanceled()
    {
        if (!State.CanReceiveInput()) return;
        StopMove();
    }

    void OnGround(bool grounded)
    {
        if (grounded)
        {
            isJump = false;
            if (!State.Is(PlayerState.Move))
                State.SetState(PlayerState.Idle);
        }

        animController.SetBool("IsGround", grounded);
    }

    void OnJump()
    {
        if (!State.CanReceiveInput() || isJump)
            return;

        isJump = true;
        animController.SetTrigger("IsJump");
        movement.Jump();
        State.SetState(PlayerState.Jump);
    }

    void TryInteract()
    {
        if (State.CurrentState == PlayerState.Die || interact.CurrentTarget == null)
            return;

        interact.TryInteract();
        State.SetState(PlayerState.Interacting);
    }

    void OnInteract(int interactType)
    {
        if (State.CurrentState == PlayerState.Die)
            return;

        StopMove();
        animController.SetInt("InteractType", interactType);

        if (interactType == 0)
        {

            var layerInfo = animator.GetCurrentAnimatorStateInfo(1);

            bool isLayerIdle = layerInfo.IsName("HumanM@Idle01 0") || layerInfo.IsTag("Idle");

 
            if (isLayerIdle)
            {
                animController.BlendLayerWeight(1, animController.GetLayerWeight(1), 0f, 0.3f);
            }

            State.SetState(PlayerState.Idle);
        }
        else
        {
            animController.SetTrigger("isInteract");
            State.SetState(PlayerState.Interacting);
            animController.BlendLayerWeight(1, 0f, 1f, 0.3f);
        }
    }

    public void OnInteractEnd()
    {
        State.SetState(PlayerState.Idle);
        animController.BlendLayerWeight(1, animController.GetLayerWeight(1), 0f, 0.3f);
    }

    public void OnDie()
    {
        input.enabled = false;
        movement.SetMoveInput(Vector3.zero);
        animController.SetTrigger("Die");
        State.SetState(PlayerState.Die);
    }

    public void OnRevive()
    {
        input.enabled = true;
        State.SetState(PlayerState.Idle);
    }

    public void StopMove()
    {
        animController.SetBool("IsMove", false);
        movement.SetMoveInput(Vector3.zero);
        State.SetState(PlayerState.Idle);
    }

    void OnStateChanged(PlayerState newState)
    {
        Debug.Log($"[PlayerController] State changed → {newState}");
    }

    public void OnSpawnTool(int contentType) => socketHandler.SpawnTool(contentType);
    public void OnDespawnTool() => socketHandler.DespawnTool();
}
