using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private ObjectPool bulletPool;
    [SerializeField] private float fireRate = 10f;

    private bool firing;
    private float nextFire;

    public void OnFirePressed() => firing = true;
    public void OnFireReleased() => firing = false;

    private void Update()
    {
        if (!firing || Time.time < nextFire) return;

        GameObject bullet = bulletPool.Get();

        bullet.transform.SetPositionAndRotation(
            firePoint.position,
            firePoint.rotation
        );

        var pooledBullet = bullet.GetComponent<PooledBullet>();
        pooledBullet.Init(bulletPool);
        pooledBullet.SetOwnerFaction(Faction.Player); 

        nextFire = Time.time + 1f / fireRate;

    }
    public void FireOnce(Faction owner)
    {
        GameObject bullet = bulletPool.Get();

        bullet.transform.SetPositionAndRotation(
            firePoint.position,
            firePoint.rotation
        );

        var pooledBullet = bullet.GetComponent<PooledBullet>();
        pooledBullet.Init(bulletPool);
        pooledBullet.SetOwnerFaction(owner);
        pooledBullet.SetOwner(GetComponentInParent<Collider2D>());
    }

}
