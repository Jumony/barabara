using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultRevolver : BaseWeaponBehaviour
{
    // Override our own function
    public override void Shoot()
    {
        Vector2 shootDirection = shootOrigin.transform.right;
        float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
        Quaternion shootRotation = Quaternion.Euler(0, 0, angle - 90);
        objectPooler.SpawnFromPool("RevolverBullets", shootOrigin.position, shootRotation);
    }
}
