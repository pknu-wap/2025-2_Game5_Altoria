using UnityEngine;

[RequireComponent(typeof(PlayerInputHandler))]
[RequireComponent(typeof(PlayerMovement))]
public class PlayerController : BaseEntityComponent
{
     PlayerInputHandler input;
     PlayerMovement movement;
  
    private bool isDead = false;

    private void Awake()
    {
        input = GetComponent<PlayerInputHandler>();
        movement = GetComponent<PlayerMovement>();
       
    }

    private void OnEnable()
    {
        BindInputEvents();
    }

    private void OnDisable()
    {
        UnbindInputEvents();
    }


    private void BindInputEvents()
    {
        input.OnMove += HandleMove;
        input.OnMoveCanceled += HandleMoveCanceled;
        input.OnJump += HandleJump;
    }

    private void UnbindInputEvents()
    {
        input.OnMove -= HandleMove;
        input.OnMoveCanceled -= HandleMoveCanceled;
        input.OnJump -= HandleJump;
    }

    private void HandleMove(Vector2 inputDir)
    {
        if (isDead) return;
        movement.SetMoveInput(new Vector3(inputDir.x, 0f, inputDir.y));
    }

    private void HandleMoveCanceled() => movement.SetMoveInput(Vector3.zero);
    private void HandleJump() => movement.Jump();
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
