using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Type", menuName = "Weapon")]
public class PlayerWeaponType : ScriptableObject
{
    public int weaponID;
    public float projectileSpeed;
    public float damage;
    public int price;
    public GameObject gunPrefab;
}
