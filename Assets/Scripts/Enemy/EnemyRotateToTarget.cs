using UnityEngine;

public class EnemyRotateToTarget : MonoBehaviour
{
    [SerializeField] private Transform target;

    private void Update()
    {
        if (!target) return;

        Vector2 dir = target.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
