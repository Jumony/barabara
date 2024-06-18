using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultFists : MonoBehaviour, IGunBehaviour
{
    public PlayerWeaponType playerWeaponType;

    public void Shoot()
    {
        return;
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
