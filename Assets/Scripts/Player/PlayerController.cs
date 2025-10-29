using System;
using System.Collections;
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

    public PlayerStateMachine State { get; private set; } = new PlayerStateMachine();

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
    void OnInteract(int interactType)
    {
        if (State.CurrentState == PlayerState.Die)
            return;
        StopMove();
        int currentType = animator.GetInteger("InteractType");
        if (currentType != interactType)
            animator.SetInteger("InteractType", interactType);

        StopAllCoroutines(); // 이전 Lerp 중단

        if (interactType == 0)
        {
            animator.SetBool("IsInteract", false);
            State.SetState(PlayerState.Idle);
            StartCoroutine(BlendLayerWeight(1f, 0f, 0.4f)); 
        }
        else
        {
            animator.SetBool("IsInteract", true);
            State.SetState(PlayerState.Interacting);
            StartCoroutine(BlendLayerWeight(0f, 1f, 0.3f));
        }
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
    
    public void StopMove()
    {
        animator.SetBool("IsMove", false);
        movement.SetMoveInput(Vector3.zero);
        State.SetState(PlayerState.Idle);
    }
    void OnStateChanged(PlayerState newState)
    {
        Debug.Log($"[PlayerController] State changed → {newState}");
    }
    IEnumerator BlendLayerWeight(float from, float to, float duration)
    {
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            float weight = Mathf.Lerp(from, to, time / duration);
            animator.SetLayerWeight(1, weight);
            yield return null;
        }
        animator.SetLayerWeight(1, to);
    }
    public void OnSpawnTool(int contentType) => socketHandler.SpawnTool(contentType);
    public void OnDespawnTool() => socketHandler.DespawnTool();

}
