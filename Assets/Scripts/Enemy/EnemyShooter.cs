using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    [SerializeField] private float fireCooldown = 1.5f;
    [SerializeField] private GunController gun;

    private float nextFireTime;

 public void TryShoot()
    {
        if (Time.time < nextFireTime)
            return;

        gun.FireOnce(Faction.Enemy);
        nextFireTime = Time.time + fireCooldown;
        Debug.Log("Enemy TryShoot called");
    }
}
