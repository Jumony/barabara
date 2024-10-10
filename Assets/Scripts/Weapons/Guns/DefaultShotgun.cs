using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultShotgun : BaseWeaponBehaviour
{
    private float[] shootAngleOffsets = { 30, 60, 90, 120, 150 };

    public override void Shoot()
    {
        Vector2 shootDirection = shootOrigin.transform.right;
        float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;

        foreach (float offset in  shootAngleOffsets)
        {
            objectPooler.SpawnFromPool("ShotgunBullets", shootOrigin.position, Quaternion.Euler(0, 0, angle - offset));
        }

    }
}
