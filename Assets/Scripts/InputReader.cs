using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class InputReader : MonoBehaviour
{
    [SerializeField] private InputActionReference move;
    [SerializeField] private InputActionReference aim;
    [SerializeField] private InputActionReference fire;
    
    public Vector2 Move { get; private set; }
    public Vector2 Aim { get; private set; }
    public event Action FirePressed;
    public event Action FireReleased;


    private void OnEnable()
    {
        move.action.Enable();
        aim.action.Enable();
        fire.action.Enable();
        move.action.performed += OnMove;
        move.action.canceled += OnMove;
        aim.action.performed += OnAim;
        aim.action.canceled += OnAim;
        fire.action.performed += OnFire;
        fire.action.canceled += OnFire;

    }

  

    private void OnDisable()
    {
        move.action.performed -= OnMove;
        move.action.canceled -= OnMove;
        aim.action.performed -= OnAim;
        aim.action.canceled -= OnAim;
        fire.action.performed -= OnFire;
        fire.action.canceled -= OnFire;
    }


    private void OnMove(InputAction.CallbackContext ctx)
    {
        Move = ctx.ReadValue<Vector2>();
    }

    private void OnAim(InputAction.CallbackContext ctx)
    {
        Aim = ctx.ReadValue<Vector2>();
    }

    private void OnFire(InputAction.CallbackContext ctx)
    {
        if (ctx.performed) FirePressed?.Invoke();
        if (ctx.canceled) FireReleased?.Invoke();
    }
}
