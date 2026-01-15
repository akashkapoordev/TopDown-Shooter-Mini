using UnityEngine;

public class EnemyAttackRange : MonoBehaviour
{
    public bool PlayerInRange { get; private set; }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            PlayerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            PlayerInRange = false;
    }
}
