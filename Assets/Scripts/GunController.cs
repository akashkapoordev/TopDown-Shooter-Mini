using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float fireRate = 10f;


    private bool isFiring;
    private float nextShotTime;

    public void OnFirePressed() => isFiring = true;
    public void OnFireReleased() => isFiring = false;

    private void Update()
    {
        if (!isFiring) return;
        if (Time.time < nextShotTime) return;

        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        nextShotTime = Time.time + 1f / fireRate;
    }
}
