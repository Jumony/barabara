using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultShotgun : MonoBehaviour, IGunBehaviour
{
    public PlayerWeaponType playerWeaponType;
    public Transform shootOrigin;
    private ObjectPooler objectPooler;
    private float[] shootAngleOffsets = { 30, 60, 90, 120, 150 };

    public float fireRate = 0.5f;
    private float nextFireTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        objectPooler = GameObject.Find("ObjectPooler").GetComponent<ObjectPooler>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime && !GameObject.Find("InventoryManager").GetComponent<RadialMenu>().menuEnabled)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    public void Shoot()
    {
        Vector2 shootDirection = shootOrigin.transform.right;
        float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;

        foreach (float offset in  shootAngleOffsets)
        {
            objectPooler.SpawnFromPool("ShotgunBullets", shootOrigin.position, Quaternion.Euler(0, 0, angle - offset));
        }

    }

    public void Reload()
    {
        return;
    }

    public PlayerWeaponType GetWeaponType()
    {
        return playerWeaponType;
    }
}
