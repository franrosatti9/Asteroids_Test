using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    private float _movementInput;
    private float _rotationInput;
    bool inputEnabled = false;
    
    public float MovementInput => _movementInput;
    public float RotationInput => _rotationInput;
    public event Action OnShootPressed;

    #region Enable/Disable
    private void OnEnable()
    {
        GameManager.Instance.OnGameStateChanged += HandleGameStateChange;
    }
    
    private void OnDisable()
    {
        GameManager.Instance.OnGameStateChanged -= HandleGameStateChange;
    }
    #endregion

    private void HandleGameStateChange(GameState state)
    {
        if (state == GameState.Playing) EnableInput(true);
        else EnableInput(false);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!inputEnabled) return;
        
        _rotationInput = context.ReadValue<Vector2>().x; 
        _movementInput = context.ReadValue<Vector2>().y;
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if(context.performed && inputEnabled) OnShootPressed?.Invoke();
    }

    public void OnPressPause(InputAction.CallbackContext context)
    {
        if(context.performed) GameManager.Instance.TogglePause();
    }

    void EnableInput(bool isEnabled)
    {
        inputEnabled = isEnabled;
    }
}
