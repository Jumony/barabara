using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGunBehaviour
{
    void Shoot();
    void Reload();
    PlayerWeaponType GetWeaponType();
}
