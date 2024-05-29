using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Type", menuName = "Weapon")]
public class PlayerWeaponType : ScriptableObject
{
    public float projectileSpeed;
    public float damage;
    public GameObject gunPrefab;
}
