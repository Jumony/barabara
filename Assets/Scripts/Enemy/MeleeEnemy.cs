using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour, IPooledObject, IDamageable
{
    [Header("Enemy Data")]
    public EnemyType meleeEnemy;

    [Header("Enemy Stats")]
    public float meleeRange = 1f;
    public float spawnWeight;

    private float health;
    private float attackDamage;
    private bool isAttacking = false;

    private Rigidbody2D rb;
    private Transform target;
    private PlayerStatManager playerStatManager;
    private EnemySpawner enemySpawner;
    private ObjectPooler objectPooler;
    private BasicPathfinding basicPathfinding;
    private Coroutine attackCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        objectPooler = ObjectPooler.Instance;
        rb = GetComponent<Rigidbody2D>();
        playerStatManager = GameObject.Find("Player").GetComponent<PlayerStatManager>();
        enemySpawner = GameObject.Find("EnemySpawner").GetComponent<EnemySpawner>();
    }

    public void OnObjectSpawn()
    {
        basicPathfinding = GetComponent<BasicPathfinding>();

        health = meleeEnemy.health;
        attackDamage = meleeEnemy.damage;

        target = GameObject.FindGameObjectWithTag("Player").transform;
        basicPathfinding.target = target;
        StartCoroutine(basicPathfinding.UpdatePath());
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, target.position) < meleeRange && !isAttacking)
        {
            attackCoroutine = StartCoroutine(MeleeAttack());
        }
    }

    private IEnumerator MeleeAttack()
    {
        isAttacking = true;
        basicPathfinding.speed = 0f;
        yield return new WaitForSeconds(2);
        playerStatManager.TakeDamage(attackDamage);
        yield return new WaitForSeconds(1);
        isAttacking = false;
        basicPathfinding.speed = meleeEnemy.moveSpeed;
        
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            // Makes sure to only spawn one coin
            if (gameObject.activeSelf)
            {
                basicPathfinding.speed = meleeEnemy.moveSpeed;
                gameObject.SetActive(false);
                objectPooler.SpawnFromPool("Coins", transform.position, transform.rotation);
                enemySpawner.EnemyDefeated();
            }
        }
    }
}
