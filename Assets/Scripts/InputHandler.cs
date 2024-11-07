using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    private float _movementInput;
    private float _rotationInput;
    
    public float MovementInput => _movementInput;
    public float RotationInput => _rotationInput;
    public event Action OnShootPressed;

    public void OnMove(InputAction.CallbackContext context)
    {
        _rotationInput = context.ReadValue<Vector2>().x; 
        _movementInput = context.ReadValue<Vector2>().y;
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if(context.performed) OnShootPressed?.Invoke();
    }
}
