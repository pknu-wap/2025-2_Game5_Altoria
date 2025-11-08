using System;
using UnityEngine;
using static Define;

[RequireComponent(typeof(PlayerInputHandler))]
public class PlayerController : BaseEntityComponent, IPlayerMovable
{
    [SerializeField] Animator animator;
    [SerializeField] SocketHandler socketHandler;
    [SerializeField] GroundChecker groundChecker;
    [SerializeField] MoveData data;
    [SerializeField] bool isJump = false;

    PlayerInputHandler input;
    PlayerInteractComponent interact;
    AnimatorControllerWrapper animController;

    public PlayerStateMachine State { get; private set; } = new PlayerStateMachine();

    public IMove Move { get; private set; }
    public IMoveData MoveData => data;

    Vector2 lastInputDir;

    void Awake()
    {
        input = GetComponent<PlayerInputHandler>();
        interact = GetComponent<PlayerInteractComponent>();
        animController = new AnimatorControllerWrapper(animator, this);
    }

    void Start()
    {
        Move = new Move();
        Move.SetEntity(this);
    }

    void Update()
    {
        if (!State.CanReceiveInput())
            return;

        if (lastInputDir.sqrMagnitude > 0.01f)
        {
            Vector3 moveDir = CalculateCameraRelativeDirection(lastInputDir);
            Move.SetMoveInput(moveDir);
        }
        else
        {
            Move.SetMoveInput(Vector3.zero);
        }

        Move.Tick();
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
        input.OnJump += OnJump;

       
        input.OnInteract += TryInteract;          
          

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

        lastInputDir = inputDir;
        animController.SetBool("IsMove", true);
        State.SetState(PlayerState.Move);
    }

    void OnMoveCanceled()
    {
        if (!State.CanReceiveInput()) return;
        lastInputDir = Vector2.zero;
        StopMove();
    }

    Vector3 CalculateCameraRelativeDirection(Vector2 inputDir)
    {
        if (Camera.main == null)
            return new Vector3(inputDir.x, 0, inputDir.y);

        Transform cam = Camera.main.transform;
        Vector3 camForward = cam.forward;
        Vector3 camRight = cam.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward.Normalize();
        camRight.Normalize();

        return (camForward * inputDir.y + camRight * inputDir.x).normalized;
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
        Move.Jump();
        State.SetState(PlayerState.Jump);
    }


    void TryInteract()
    {
        if (State.CurrentState == PlayerState.Die || interact.CurrentTarget == null) return;


        interact.TryInteract(this);
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
                animController.BlendLayerWeight(1, animController.GetLayerWeight(1), 0f, 0.3f);

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
        lastInputDir = Vector2.zero;
        Move.SetMoveInput(Vector3.zero);
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
        lastInputDir = Vector2.zero;
        animController.SetBool("IsMove", false);
        Move.SetMoveInput(Vector3.zero);
        State.SetState(PlayerState.Idle);
    }

    void OnStateChanged(PlayerState newState)
    {
        Debug.Log($"[PlayerController] State changed → {newState}");
    }

    public void OnSpawnTool(int contentType) => socketHandler.SpawnTool(contentType);
    public void OnDespawnTool() => socketHandler.DespawnTool();
}
