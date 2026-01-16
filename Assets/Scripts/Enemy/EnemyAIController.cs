using UnityEngine;

public class EnemyAIController : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform player;

    [Header("Ranges")]
    [SerializeField] private float chaseRange = 6f;
    [SerializeField] private float attackRange = 3.5f;

    [Header("Gun Aiming")]
    [SerializeField] private Transform gunPivot; // 🔥 THIS ROTATES
    [SerializeField] private float rotationSpeed = 720f; // degrees/sec
    [SerializeField] private float fireAngleThreshold = 5f; // degrees

    [Header("Strafing")]
    [SerializeField] private float strafeSpeed = 1.5f;
    [SerializeField] private float strafeSwitchTime = 1.5f;

    [Header("Colliders")]
    [SerializeField] private Collider2D enemyCollider;
    [SerializeField] private Collider2D playerCollider;

    private EnemyState currentState = EnemyState.Idle;
    private EnemyMovement movement;
    private EnemyShooter shooter;

    private float nextStrafeSwitch;
    private int strafeDirection = 1;

    private void Awake()
    {
        movement = GetComponent<EnemyMovement>();
        shooter = GetComponent<EnemyShooter>();
    }

    private void Update()
    {
        if (!player) return;

        float distance = GetColliderAwareDistance();

        HandleStateTransitions(distance);
        HandleStateActions(distance);
    }

    // -------------------- STATE TRANSITIONS --------------------

    private void HandleStateTransitions(float distance)
    {
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
                if (distance > attackRange)
                    ChangeState(EnemyState.Chase);
                break;
        }
    }

    // -------------------- STATE ACTIONS --------------------

    private void HandleStateActions(float distance)
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                movement.SetMoveDirection(Vector2.zero);
                break;

            case EnemyState.Chase:
                movement.SetMoveDirection(player.position - transform.position);
                break;

            case EnemyState.Attack:
                HandleAttack();
                break;
        }
    }

    // -------------------- ATTACK LOGIC --------------------

    private void HandleAttack()
    {
        movement.SetMoveDirection(Vector2.zero);

        // Rotate gun toward player
        bool aligned = RotateGunTowardsPlayerSmooth();

        // Fire ONLY when gun is aligned
        if (aligned)
            shooter.TryShoot();

        // Optional strafing
        Vector2 toPlayer = (player.position - transform.position).normalized;
        Vector2 perpendicular = new Vector2(-toPlayer.y, toPlayer.x);

        UpdateStrafeDirection();
        movement.SetMoveDirection(perpendicular * strafeDirection * strafeSpeed);
    }

    // -------------------- GUN ROTATION --------------------

    private bool RotateGunTowardsPlayerSmooth()
    {
        Vector2 dir = (player.position - gunPivot.position).normalized;
        float targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        float currentAngle = gunPivot.eulerAngles.z;
        float newAngle = Mathf.MoveTowardsAngle(
            currentAngle,
            targetAngle,
            rotationSpeed * Time.deltaTime
        );

        gunPivot.rotation = Quaternion.Euler(0, 0, newAngle);

        float angleDiff = Mathf.Abs(Mathf.DeltaAngle(newAngle, targetAngle));
        return angleDiff <= fireAngleThreshold;
    }

    // -------------------- HELPERS --------------------

    private float GetColliderAwareDistance()
    {
        float rawDistance = Vector2.Distance(transform.position, player.position);

        float colliderOffset =
            enemyCollider.bounds.extents.x +
            playerCollider.bounds.extents.x;

        return rawDistance - colliderOffset;
    }

    private void UpdateStrafeDirection()
    {
        if (Time.time >= nextStrafeSwitch)
        {
            strafeDirection = Random.value > 0.5f ? 1 : -1;
            nextStrafeSwitch = Time.time + strafeSwitchTime;
        }
    }

    private void ChangeState(EnemyState newState)
    {
        if (currentState == newState) return;
        currentState = newState;
    }

    // -------------------- DEBUG --------------------

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
