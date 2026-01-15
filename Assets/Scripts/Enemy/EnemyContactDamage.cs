using UnityEngine;

public class EnemyContactDamage : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    [SerializeField] private float damageCooldown = 1f;

    private float lastDamageTime;

    private void OnTriggerEnter2D(Collider2D other)
    {
        TryDealDamage(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        TryDealDamage(other);
    }

    private void TryDealDamage(Collider2D collider)
    {
        if (Time.time < lastDamageTime + damageCooldown)
            return;

        if (collider.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.TakeDamage(damage);
            lastDamageTime = Time.time;
        }
    }
}
