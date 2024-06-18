using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
public class EnemyType : ScriptableObject
{
    public GameObject prefab;
    public float health;
    public float moveSpeed;
    public float damage;
}
