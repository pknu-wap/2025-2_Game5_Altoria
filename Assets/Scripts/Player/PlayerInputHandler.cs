using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerInputHandler : MonoBehaviour
{
     InputSystem_Actions inputActions;

    public event Action<Vector2> OnMove;
    public event Action OnMoveCanceled, OnJump, OnAttack, OnInteract;

     void Awake()
    {
        inputActions = new InputSystem_Actions();
    }

     void OnEnable()
    {
        inputActions.Enable();
        BindInputs();
    }

     void OnDisable()
    {
        UnbindInputs();
        inputActions.Disable();
    }

     void BindInputs()
    {
        inputActions.Player.Move.performed += OnMovePerformed;
        inputActions.Player.Move.canceled += OnMoveCanceledPerformed;
        inputActions.Player.Jump.performed += OnJumpPerformed;
        inputActions.Player.Attack.performed += OnAttackPerformed;
        inputActions.Player.Interact.performed += OnInteractPerformed;
    }

    void UnbindInputs()
    {
        inputActions.Player.Move.performed -= OnMovePerformed;
        inputActions.Player.Move.canceled -= OnMoveCanceledPerformed;
        inputActions.Player.Jump.performed -= OnJumpPerformed;
        inputActions.Player.Attack.performed -= OnAttackPerformed;
        inputActions.Player.Interact.performed -= OnInteractPerformed;
    }

  
     void OnMovePerformed(InputAction.CallbackContext ctx) => OnMove?.Invoke(ctx.ReadValue<Vector2>());
     void OnMoveCanceledPerformed(InputAction.CallbackContext ctx) => OnMoveCanceled?.Invoke();
     void OnJumpPerformed(InputAction.CallbackContext ctx) => OnJump?.Invoke();
     void OnAttackPerformed(InputAction.CallbackContext ctx) => OnAttack?.Invoke();
     void OnInteractPerformed(InputAction.CallbackContext ctx) => OnInteract?.Invoke();
}
