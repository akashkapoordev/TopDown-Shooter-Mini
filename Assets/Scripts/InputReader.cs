using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class InputReader : MonoBehaviour
{
    [SerializeField] private InputActionReference move;
    [SerializeField] private InputActionReference aim;
    [SerializeField] private InputActionReference fire;
    [SerializeField] private InputActionReference dash;
    
    public Vector2 Move { get; private set; }
    public Vector2 Aim { get; private set; }
    public event Action FirePressed;
    public event Action FireReleased;
    public event Action DashPressed;


    private void OnEnable()
    {
        move.action.Enable();
        aim.action.Enable();
        fire.action.Enable();
        dash.action.Enable();
        move.action.performed += OnMove;
        move.action.canceled += OnMove;
        aim.action.performed += OnAim;
        aim.action.canceled += OnAim;
        fire.action.performed += OnFire;
        fire.action.canceled += OnFire;
        dash.action.performed += OnDash;

    }

  

    private void OnDisable()
    {
        move.action.performed -= OnMove;
        move.action.canceled -= OnMove;
        aim.action.performed -= OnAim;
        aim.action.canceled -= OnAim;
        fire.action.performed -= OnFire;
        fire.action.canceled -= OnFire;
        dash.action.performed -= OnDash;

        move.action.Disable();
        aim.action.Disable();
        fire.action.Disable();
        dash.action.Disable();
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
    private void OnDash(InputAction.CallbackContext ctx)
    {
        Debug.Log("DASH INPUT FIRED");
        DashPressed?.Invoke();
    }
}
