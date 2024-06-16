using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingController  : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed = 20f;
    [SerializeField] private float fireRate = 0.5f;

    private float _nextFireTime;

    private void Update()
    {
        if (CanShoot() && Time.time > _nextFireTime)
        {
            Shoot();
            _nextFireTime = Time.time + fireRate;
        }
    }

    protected virtual bool CanShoot()
    {
        // Реализация в дочерних классах
        return false;
    }

    protected virtual void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        // Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        // if (rb != null)
        // {
        //     rb.velocity = firePoint.up * bulletSpeed;
        // }
    }
}
