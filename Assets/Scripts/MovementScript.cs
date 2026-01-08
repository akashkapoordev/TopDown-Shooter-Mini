using UnityEngine;

public class MovementScript : MonoBehaviour
{
    [SerializeField] private InputReader input;
    [SerializeField] private float movementSpeed = 6f;


    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Vector2 move = input.Move;

        if (move.sqrMagnitude > 1f) move.Normalize();

        rb.linearVelocity = move * movementSpeed;
    }
}
