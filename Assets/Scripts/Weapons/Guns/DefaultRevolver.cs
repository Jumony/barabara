using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultRevolver : MonoBehaviour, IGunBehaviour
{
    public Transform shootOrigin;
    private ObjectPooler objectPooler;

    private void Start()
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
        Quaternion shootRotation = Quaternion.Euler(0, 0, angle - 90);
        objectPooler.SpawnFromPool("Projectiles", shootOrigin.position, shootRotation);
    }

    public void Reload()
    {
        return;
    }
}
