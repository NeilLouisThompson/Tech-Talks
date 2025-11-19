using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public int poolSize = 20;
    private List<GameObject> bulletPool;
    public float bulletSpeed = 20f;
    private float shootCooldown = 0.2f;
    private float lastShotTime;

    void Start()
    {
        bulletPool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.SetActive(false);
            bulletPool.Add(bullet);
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.time > lastShotTime + shootCooldown)
        {
            lastShotTime = Time.time;
            Shoot();
        }
    }

    void Shoot()
    {
        GameObject bullet = GetPooledBullet();
        if (bullet != null)
        {
            bullet.transform.position = firePoint.position;
            bullet.transform.rotation = firePoint.rotation;
            bullet.SetActive(true);
            bullet.GetComponent<Rigidbody>().linearVelocity = firePoint.forward * bulletSpeed;
        }
    }

    GameObject GetPooledBullet()
    {
        foreach (var bullet in bulletPool)
        {
            if (!bullet.activeInHierarchy)
            {
                return bullet;
            }
        }
        return null;
    }
}
