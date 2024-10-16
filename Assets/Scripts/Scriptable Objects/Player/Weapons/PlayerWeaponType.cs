using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Weapon Type", menuName = "Weapon")]
public class PlayerWeaponType : ScriptableObject
{
    public int weaponID;
    public float projectileSpeed;
    public float projectileLifetime;
    public float fireRate;
    public float damage;
    public int price;
    public GameObject gunPrefab;
    public Sprite sprite;
}
