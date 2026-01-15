using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyKnockback : MonoBehaviour, IKnockbackable
{
    [SerializeField] private float knockbackResistance = 1f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void ApplyKnockback(Vector2 direction, float force)
    {
        rb.AddForce(direction.normalized * force / knockbackResistance,
                    ForceMode2D.Impulse);
    }
}
