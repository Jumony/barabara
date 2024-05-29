using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultShotgun : MonoBehaviour, IGunBehaviour
{

    public Transform shootOrigin;
    private ObjectPooler objectPooler;

    // Start is called before the first frame update
    void Start()
    {
        objectPooler = GameObject.Find("ObjectPooler").GetComponent<ObjectPooler>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        Vector2 shootDirection = shootOrigin.transform.right;
        float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
        Quaternion upAdjustment1 = Quaternion.Euler(0, 0, angle - 60);
        Quaternion upAdjustment2 = Quaternion.Euler(0, 0, angle - 30);
        Quaternion straightShootRotation = Quaternion.Euler(0, 0, angle - 90);
        Quaternion downAdjustment1 = Quaternion.Euler(0, 0, angle - 120);
        Quaternion downAdjustment2 = Quaternion.Euler(0, 0, angle - 150);
        objectPooler.SpawnFromPool("Shotgun", shootOrigin.position, upAdjustment1);
        objectPooler.SpawnFromPool("Shotgun", shootOrigin.position, upAdjustment2);
        objectPooler.SpawnFromPool("Shotgun", shootOrigin.position, straightShootRotation);
        objectPooler.SpawnFromPool("Shotgun", shootOrigin.position, downAdjustment1);
        objectPooler.SpawnFromPool("Shotgun", shootOrigin.position, downAdjustment2);
    }

    public void Reload()
    {

    }
}
