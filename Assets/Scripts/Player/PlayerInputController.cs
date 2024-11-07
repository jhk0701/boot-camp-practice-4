using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    public event Action<Vector2> OnMoveEvent;
    public event Action OnJumpEvent;
    public event Action<Vector2> OnLookEvent;
    public event Action OnAttackEvent;
    public event Action OnMagicEvent;
    public event Action OnInteractEvent;
    public event Action OnToggleInventoryEvent;
    public event Action OnToggleSettingEvent;

    public void OnMove(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
            OnMoveEvent?.Invoke(context.ReadValue<Vector2>());
        else if(context.phase == InputActionPhase.Canceled)
            OnMoveEvent?.Invoke(Vector2.zero);
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if(context.started)
            OnJumpEvent?.Invoke();
    }
    
    public void OnLook(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
            OnLookEvent?.Invoke(context.ReadValue<Vector2>());
        else if(context.phase == InputActionPhase.Canceled)
            OnLookEvent?.Invoke(Vector2.zero);
    }

    public void OnMagic(InputAction.CallbackContext context)
    {
        if(context.started)
            OnMagicEvent?.Invoke();
    }
    public void OnAttack(InputAction.CallbackContext context)
    {
        if(context.started)
            OnAttackEvent?.Invoke();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            OnInteractEvent?.Invoke();
        }
    }

    public void OnToggleSetting(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            OnToggleSettingEvent?.Invoke();
        }
    }
    
    public void OnToggleInventory(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            OnToggleInventoryEvent?.Invoke();
        }
    }
}

