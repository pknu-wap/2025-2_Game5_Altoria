using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System;

public class PlayerInputHandler : MonoBehaviour
{
    InputSystem_Actions.PlayerActions playerActions;
    InputSystem_Actions.UIActions uiActions;

    public event Action<Vector2> OnMove;
    public event Action OnMoveCanceled, OnJump, OnAttack, OnInteract;
    public event Action<Vector2> OnLook;
    public event Action<bool> OnCursorLockChanged;

    bool isCursorLocked;
    bool isAltMode;
    bool isOpen;
    void Awake()
    {
        var input = new InputSystem_Actions();
        playerActions = input.Player;
        uiActions = input.UI;
    }

    void OnEnable()
    {
        BindInputs();
        playerActions.Enable();
        uiActions.Enable();
        ForceLockCursor();
    }

    void OnDisable()
    {
        UnbindInputs();
        playerActions.Disable();
        uiActions.Disable();
        ForceUnlockCursor();
    }

    void BindInputs()
    {
        playerActions.Move.performed += OnMovePerformed;
        playerActions.Move.canceled += OnMoveCanceledPerformed;
        playerActions.Jump.performed += OnJumpPerformed;
        playerActions.Attack.performed += OnAttackPerformed;
        playerActions.Interact.performed += OnInteractPerformed;
        playerActions.Look.performed += OnLookPerformed;
        playerActions.Look.canceled += OnLookCanceled;

        uiActions.Inventory.performed += _ => OpenInventory();
        uiActions.MainMenu.performed += _ => OpenMainMenu();

     
        uiActions.AltCursor.performed += _ => ToggleAltMode();
    }

    void UnbindInputs()
    {
        playerActions.Move.performed -= OnMovePerformed;
        playerActions.Move.canceled -= OnMoveCanceledPerformed;
        playerActions.Jump.performed -= OnJumpPerformed;
        playerActions.Attack.performed -= OnAttackPerformed;
        playerActions.Interact.performed -= OnInteractPerformed;
        playerActions.Look.performed -= OnLookPerformed;
        playerActions.Look.canceled -= OnLookCanceled;

        uiActions.Inventory.performed -= _ => OpenInventory();
        uiActions.MainMenu.performed -= _ => OpenMainMenu();
        uiActions.AltCursor.performed -= _ => ToggleAltMode();
    }

    void Update()
    {
        HandleAltClick();
    }

  
    void ToggleAltMode()
    {
        isAltMode = !isAltMode;

        if (isAltMode)
            ForceUnlockCursor();
        else
            ForceLockCursor();
    }

    void HandleAltClick()
    {
      
        if (isAltMode && Mouse.current.leftButton.wasPressedThisFrame && !IsPointerOverUI())
        {
            isAltMode = false;
            ForceLockCursor();
        }
    }

 
    void ForceLockCursor()
    {
        isCursorLocked = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        OnCursorLockChanged?.Invoke(true);
    }

    void ForceUnlockCursor()
    {
        isCursorLocked = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        OnCursorLockChanged?.Invoke(false);
    }

  
    void OpenInventory()
    {
        ForceUnlockCursor();
        
        Manager.UI.ShowPopup<InventoryUI>();
    }
        
       
        
  
    void OpenMainMenu()
    {
        if (Manager.UI.IsAnyPopUp()) Manager.UI.ClosePopup();
        else
        {
            Manager.UI.ShowPopup<MainMenuPopUp>();
            ForceUnlockCursor();
        }
    }

    bool IsPointerOverUI() =>
        EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();

    void OnMovePerformed(InputAction.CallbackContext ctx) => OnMove?.Invoke(ctx.ReadValue<Vector2>());
    void OnMoveCanceledPerformed(InputAction.CallbackContext ctx) => OnMoveCanceled?.Invoke();
    void OnJumpPerformed(InputAction.CallbackContext ctx) => OnJump?.Invoke();
    void OnAttackPerformed(InputAction.CallbackContext ctx) => OnAttack?.Invoke();
    void OnInteractPerformed(InputAction.CallbackContext ctx) => OnInteract?.Invoke();

    void OnLookPerformed(InputAction.CallbackContext ctx)
    {
        if (!isCursorLocked) return;
        OnLook?.Invoke(ctx.ReadValue<Vector2>());
    }

    void OnLookCanceled(InputAction.CallbackContext ctx) => OnLook?.Invoke(Vector2.zero);
}
