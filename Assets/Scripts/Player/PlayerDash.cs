using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerDash : MonoBehaviour
{
    [SerializeField] private InputReader input;
    [SerializeField] private float dashSpeed = 15f;
    [SerializeField] private float dashDuration = 0.15f;
    [SerializeField] private float dashCooldown = 0.5f;
    public bool IsDashing { get; private set; }


    private Rigidbody2D rb;
    private float dashEndTime;
    private float nextDashTime;
    private bool isDashing;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        input.DashPressed += TryDash;
    }

    private void OnDisable()
    {
        input.DashPressed -= TryDash;
    }

    private void TryDash()
    {
        Debug.Log("TRY DASH CALLED");
        if (Time.time < nextDashTime || IsDashing)
            return;

        Vector2 dashDir = input.Move.sqrMagnitude > 0.1f
            ? input.Move.normalized
            : transform.right;

        IsDashing = true;
        dashEndTime = Time.time + dashDuration;
        nextDashTime = Time.time + dashCooldown;

        rb.linearVelocity = dashDir * dashSpeed;
    }

    private void FixedUpdate()
    {
        if (!IsDashing) return;
        if (IsDashing)
            Debug.Log("DASHING VELOCITY: " + rb.linearVelocity);
        if (Time.time >= dashEndTime)
        {
            IsDashing = false;
            rb.linearVelocity = Vector2.zero;
        }
    }

}
