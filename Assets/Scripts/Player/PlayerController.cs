using GameInteract;
using System;
using UnityEngine;
using UnityEngine.AI;
using static Define;

[RequireComponent(typeof(PlayerInputHandler))]
public class PlayerController : BaseEntityComponent, IPlayerMovable, IMoveInput, IInteractInput, IRidingInput
{
    [SerializeField] Animator animator;
    [SerializeField] SocketHandler socketHandler;
    [SerializeField] GroundChecker groundChecker;
    [SerializeField] RiderComponent rider;
    [SerializeField] PlayerCameraHandler cameraHandler;
    [SerializeField] MoveData data;
    [SerializeField] bool isJump = false;

    PlayerInputHandler input;
    PlayerInteractComponent interact;
    AnimatorControllerWrapper animController;
    InputBinder inputBinder;
    MoveHandler moveHandler;

    public PlayerStateMachine State { get; private set; } = new PlayerStateMachine();
    public IMove Move { get; private set; }
    public IMoveData MoveData => data;

    void Awake()
    {
        input = GetComponent<PlayerInputHandler>();
        interact = GetComponent<PlayerInteractComponent>();
        animController = new AnimatorControllerWrapper(animator, this);
        inputBinder = new InputBinder(input);
    }

    void Start()
    {
        Move = new Move();
        Move.SetEntity(this);

        moveHandler = new MoveHandler(Move);
        inputBinder.Initialize(this);
    }

    void Update()
    {
        if (!State.CanReceiveInput()) return;

        moveHandler.Tick(); 
        animController.SetBool("IsMove", MoveIsActive());
    }

    bool MoveIsActive()
    {
      
        if (Move is Move concreteMove && concreteMove.GetTransform().GetComponent<CharacterController>() != null)
            return concreteMove.GetTransform().GetComponent<CharacterController>().velocity.magnitude > 0.05f;
        return false;
    }

    void OnEnable()
    {
        groundChecker.OnGroundedChanged += OnGround;
        State.OnStateChanged += OnStateChanged;
        interact.InteractSystem.InteractInvoke += OnInteract;
        rider.OnRideChanged += OnRideChanged;
    }

    void OnDisable()
    {
        groundChecker.OnGroundedChanged -= OnGround;
        State.OnStateChanged -= OnStateChanged;
        interact.InteractSystem.InteractInvoke -= OnInteract;
        inputBinder.Unbind();
    }

    #region Move Input
    public void OnMoveInput(Vector2 inputDir)
    {
        if (!State.CanReceiveInput()) return;

        moveHandler.SetInput(inputDir);
        animController.SetBool("IsMove", true);
        State.SetState(PlayerState.Move);
    }

    public void OnMoveCancel()
    {
        if (!State.CanReceiveInput()) return;

        moveHandler.SetInput(Vector2.zero);
        StopMove();
    }

    public void StopMove()
    {
        animController.SetBool("IsMove", false);
        Move.SetMoveInput(Vector3.zero);
        State.SetState(PlayerState.Idle);
    }
    #endregion

    #region Jump
    void OnGround(bool grounded)
    {
        if (grounded)
        {
            isJump = false;
            if (!State.HasState(PlayerState.Move))
                State.SetState(PlayerState.Idle);
        }
        animController.SetBool("IsGround", grounded);
    }

    public void OnJumpInput()
    {
        if (!State.CanReceiveInput() || isJump)
            return;

        isJump = true;
        animController.SetTrigger("IsJump");
        Move.Jump();
        State.SetState(PlayerState.Jump);
    }
    #endregion

    #region Interact
    public void TryInteract()
    {
        if (State.CurrentState == PlayerState.Die || interact.CurrentTarget == null) return;
        interact.TryInteract(this);
        State.SetState(PlayerState.Interacting);
    }

    void OnInteract(int interactType)
    {
        if (State.CurrentState == PlayerState.Die) return;

        StopMove();
        animController.SetInt("InteractType", interactType);

        if (interactType == 0)
        {
            var layerInfo = animator.GetCurrentAnimatorStateInfo(1);
            bool isLayerIdle = layerInfo.IsName("HumanM@Idle01 0") || layerInfo.IsTag("Idle");
            if (isLayerIdle)
                animController.BlendLayerWeight(1, animController.GetLayerWeight(1), 0f, 0.3f);
            cameraHandler.SetDefaultCamera();
            State.SetState(PlayerState.Idle);
        }
        else
        {
            ContentType type = (ContentType)interactType;
            cameraHandler.SetCamera(type.ToString()+"Camera");
            animController.SetTrigger("isInteract");
            State.AddState(PlayerState.Interacting);
            animController.BlendLayerWeight(1, 0f, 1f, 0.3f);
        }
    }

    public void OnInteractEnd()
    {
        State.RemoveState(PlayerState.Idle);
        animController.BlendLayerWeight(1, animController.GetLayerWeight(1), 0f, 0.3f);
    }
    #endregion

    #region Die
    public void OnDie()
    {
        input.enabled = false;
        moveHandler.SetInput(Vector2.zero);
        Move.SetMoveInput(Vector3.zero);
        animController.SetTrigger("Die");
        State.SetState(PlayerState.Die);
    }

    public void OnRevive()
    {
        input.enabled = true;
        State.SetState(PlayerState.Idle);
    }
    #endregion

    #region Riding
    public void TryRiding() => rider.TryRide(this);

    void OnRideChanged(bool mounted, IRiding mount)
    {
        Debug.Log($"OnRideChanged {mounted}");

        if (Move is Move moveComp)
            moveComp.SetMovementLock(mounted);

        int ridingLayerIndex = 2; 
        float targetWeight = mounted ? 1f : 0f;
        animController.BlendLayerWeight(ridingLayerIndex, animController.GetLayerWeight(ridingLayerIndex), targetWeight, 0.3f);

        if (mounted)
        {
            if (mount is IMoveInput mountInput)
                inputBinder.Bind(mountInput);
            cameraHandler.SetCamera("RidingCamera");
            animController.SetBool("IsRiding", true);
            State.AddState(PlayerState.Riding);

            Debug.Log($"[PlayerController] Mounted on {mount}");
        }
        else
        {
            inputBinder.Bind(this);
            animController.SetBool("IsRiding", false);
            cameraHandler.SetDefaultCamera();
            State.RemoveState(PlayerState.Riding);

            Debug.Log("[PlayerController] Dismounted");
        }
    }

    #endregion

    void OnStateChanged(PlayerState newState)
    {
        Debug.Log($"[PlayerController] State changed → {newState}");
    }

    public void OnSpawnTool(int contentType) => socketHandler.SpawnTool(contentType);
    public void OnDespawnTool() => socketHandler.DespawnTool();
}
