using UnityEngine;

public class EnemyAIController : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform player;

    [Header("Ranges")]
    [SerializeField] private float chaseRange = 6f;
    [SerializeField] private float attackRange = 3.5f;

    [Header("Attack")]
    [SerializeField] private float attackStateDuration = 1.2f;

    [Header("Strafing")]
    [SerializeField] private float strafeSpeed = 1.5f;
    [SerializeField] private float strafeSwitchTime = 1.5f;

    private float nextStrafeSwitch;
    private int strafeDirection = 1; // 1 = right, -1 = left


    [SerializeField] private Collider2D enemyCollider;
    [SerializeField] private Collider2D playerCollider;


    private EnemyState currentState;
    private EnemyMovement movement;
    private EnemyShooter shooter;

    private Vector2 lockedAimDirection;
    private float attackStateEndTime;

    private void Awake()
    {
        movement = GetComponent<EnemyMovement>();
        shooter = GetComponent<EnemyShooter>();
        currentState = EnemyState.Idle;
    }

    private void Update()
    {
        if (!player) return;

        float rawDistance = Vector2.Distance(transform.position, player.position);

        float colliderOffset =
            enemyCollider.bounds.extents.x +
            playerCollider.bounds.extents.x;

        float distance = rawDistance - colliderOffset;
        if (currentState == EnemyState.Attack && distance < attackRange * 0.8f)
        {
            ChangeState(EnemyState.Chase);
        }


        switch (currentState)
        {
            case EnemyState.Idle:
                if (distance <= chaseRange)
                    ChangeState(EnemyState.Chase);
                break;

            case EnemyState.Chase:
                if (distance <= attackRange)
                    ChangeState(EnemyState.Attack);
                else if (distance > chaseRange)
                    ChangeState(EnemyState.Idle);
                break;

            case EnemyState.Attack:
                if (Time.time >= attackStateEndTime && distance > attackRange)
                    ChangeState(EnemyState.Chase);
                break;
        }

        if (currentState != EnemyState.Attack)
        {
            RotateTowardsPlayer();
        }
        else
        {
            transform.right = lockedAimDirection;
        }

        HandleState(distance);
    }

    private void HandleState(float distance)
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                movement.SetMoveDirection(Vector2.zero);
                break;

            case EnemyState.Chase:
                {
                    Vector2 toPlayer = player.position - transform.position;

                    if (distance > attackRange)
                    {
                        movement.SetMoveDirection(toPlayer);
                    }
                    else
                    {

                        movement.SetMoveDirection(Vector2.zero);
                        ChangeState(EnemyState.Attack);
                    }
                    break;
                }
            case EnemyState.Attack:
                {
                    // Always shoot
                    shooter.TryShoot();

                    // Strafe movement
                    Vector2 toPlayer = (player.position - transform.position).normalized;
                    Vector2 perpendicular = new Vector2(-toPlayer.y, toPlayer.x);

                    UpdateStrafeDirection();

                    movement.SetMoveDirection(perpendicular * strafeDirection * strafeSpeed);
                    break;
                }
        }
    }

    private void ChangeState(EnemyState newState)
    {
        if (currentState == newState) return;

        currentState = newState;

        if (newState == EnemyState.Attack)
        {
           
            lockedAimDirection =
                (player.position - transform.position).normalized;

            attackStateEndTime = Time.time + attackStateDuration;
        }
    }

    private void RotateTowardsPlayer()
    {
        Vector2 dir = player.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void OnDrawGizmosSelected()
    {
        if (!player) return;

        // Chase range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        // Attack range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // Actual collider-aware stop distance (visual aid)
        if (enemyCollider && playerCollider)
        {
            float colliderOffset =
                enemyCollider.bounds.extents.x +
                playerCollider.bounds.extents.x;

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, colliderOffset);
        }
    }

    private void UpdateStrafeDirection()
    {
        if (Time.time >= nextStrafeSwitch)
        {
            strafeDirection = Random.value > 0.5f ? 1 : -1;
            nextStrafeSwitch = Time.time + strafeSwitchTime;
        }
    }


}
