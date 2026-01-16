using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 6f;
    [SerializeField] private PlayerDash dash;

    private Rigidbody2D rb;
    private Vector2 moveInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetMoveInput(Vector2 move)
    {
        moveInput = move.sqrMagnitude > 1f ? move.normalized : move;
    }

    private void FixedUpdate()
    {

        if (dash != null && dash.IsDashing)
            return;

        rb.linearVelocity = moveInput * speed;

    }
}
