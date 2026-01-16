using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PooledBullet : MonoBehaviour
{
    [SerializeField] private float speed = 15f;
    [SerializeField] private float lifeTime = 2f;
    [SerializeField] private int damage = 10;
    [SerializeField] private float knockbackForce = 5f;


    private Rigidbody2D rb;
    private ObjectPool pool;
    private float timer;
    private bool isReturned;
    private Faction ownerFaction;
    private Collider2D ownerCollider;

    public void SetOwner(Collider2D owner)
    {
        ownerCollider = owner;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Init(ObjectPool poolRef)
    {
        pool = poolRef;
    }

    public void SetOwnerFaction(Faction faction)
    {
        ownerFaction = faction;
    }

    private void OnEnable()
    {
        isReturned = false;
        ownerFaction = Faction.Player; 

        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        timer = lifeTime;

        if (ownerCollider != null)
        {
            Collider2D bulletCollider = GetComponent<Collider2D>();
            Physics2D.IgnoreCollision(bulletCollider, ownerCollider, true);
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = transform.right * speed;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            ReturnToPool();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isReturned) return;

        if (other.TryGetComponent<Health>(out var health))
        {
            if (health.GetFaction() != ownerFaction)
            {
                // DAMAGE
                health.TakeDamage(damage);

                // KNOCKBACK (optional)
                if (other.TryGetComponent<IKnockbackable>(out var knockbackable))
                {
                    Vector2 direction =
                        (other.transform.position - transform.position).normalized;

                    knockbackable.ApplyKnockback(direction, knockbackForce);
                }

                ReturnToPool();
            }
        }
    }


    private void ReturnToPool()
    {
        if (isReturned) return;

        isReturned = true;
        pool.Return(gameObject);
    }
    private void OnDisable()
    {
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        if (ownerCollider != null)
        {
            Collider2D bulletCollider = GetComponent<Collider2D>();
            Physics2D.IgnoreCollision(bulletCollider, ownerCollider, false);
        }

        ownerCollider = null;
    }

}
