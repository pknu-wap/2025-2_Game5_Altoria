using System;
using UnityEngine;

[RequireComponent(typeof(PlayerInputHandler))]
[RequireComponent(typeof(PlayerMovement))]
public class PlayerController : BaseEntityComponent
{
    [SerializeField] Animator animator;
   
     PlayerInputHandler input;
     PlayerMovement movement;
    PlayerInteractComponent interact;
    bool isDead = false;

    void Awake()
    {
        input = GetComponent<PlayerInputHandler>();
        movement = GetComponent<PlayerMovement>();
        interact=GetComponent<PlayerInteractComponent>();   
        GetComponent<GroundChecker>().OnGroundedChanged += OnGround;
    }

    

    void OnEnable() => BindInputEvents();

    void OnDisable() => UnbindInputEvents();


    void BindInputEvents()
    {
        input.OnMove += HandleMove;
        input.OnMoveCanceled += HandleMoveCanceled;
        input.OnInteract += HandleInteract;
        input.OnJump += HandleJump;
    }

 
    void UnbindInputEvents()
    {
        input.OnMove -= HandleMove;
        input.OnMoveCanceled -= HandleMoveCanceled;
        input.OnJump -= HandleJump;
        input.OnInteract-= HandleInteract; 
    }

     void HandleMove(Vector2 inputDir)
    {
        if (isDead) return;
        movement.SetMoveInput(new Vector3(inputDir.x, 0f, inputDir.y));
        animator.SetBool("IsMove", true);
    }

    void HandleMoveCanceled()
    {
        animator.SetBool("IsMove", false);
        movement.SetMoveInput(Vector3.zero);
    }
     void HandleJump()
    {
        if (!movement.IsGrounded) return;
   
        animator.SetTrigger("IsJump");
        movement.Jump();
    }
    void OnGround(bool ground)
    {
        animator.SetBool("IsGround",ground);
    }
    void HandleInteract()
    {
        Debug.Log("Interact");
        interact.TryInteract();
    }

 
    public void Die()
    {
        isDead = true;
        input.enabled = false;
        movement.SetMoveInput(Vector3.zero);
    }

    public void Revive()
    {
        isDead = false;
        input.enabled = true;
    }
}
