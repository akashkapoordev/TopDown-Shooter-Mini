using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private Faction faction;

    // Only used for Player
    [SerializeField] private float invulnerableTime = 0.5f;

    private int currentHealth;
    private float invulnerableUntil;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public Faction GetFaction() => faction;

    public void TakeDamage(int damage)
    {
        // Apply I-frames ONLY to Player
        if (faction == Faction.Player && Time.time < invulnerableUntil)
            return;

        currentHealth -= damage;
        Debug.Log($"{gameObject.name} HP: {currentHealth}");

        // Start invulnerability ONLY for Player
        if (faction == Faction.Player)
            invulnerableUntil = Time.time + invulnerableTime;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        gameObject.SetActive(false);
    }
}
