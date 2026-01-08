using UnityEngine;
using UnityEngine.InputSystem;
public class InputReader : MonoBehaviour
{
    [SerializeField] private InputActionReference move;
    
    public Vector2 Move { get; private set; }


    private void OnEnable()
    {
        move.action.Enable();
        move.action.performed += OnMove;
        move.action.canceled += OnMove;

    }

    private void OnDisable()
    {
        move.action.performed -= OnMove;
        move.action.canceled -= OnMove;
    }


    private void OnMove(InputAction.CallbackContext ctx)
    {
        Move = ctx.ReadValue<Vector2>();
    }
}
