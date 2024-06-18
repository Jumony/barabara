using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour, IPooledObject, IDamageable
{
    public EnemyType meleeEnemy;
    public float meleeRange = 1f;
    public float health;
    public float speed;
    public float attackDamage;
    public float spawnWeight;

    private Rigidbody2D rb;
    private Transform target;
    private PlayerStatManager playerStatManager;
    private EnemySpawner enemySpawner;
    private ObjectPooler objectPooler;
    private BasicPathfinding basicPathfinding;

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
        speed = meleeEnemy.moveSpeed;
        attackDamage = meleeEnemy.damage;

        target = GameObject.FindGameObjectWithTag("Player").transform;
        basicPathfinding.target = target;
        StartCoroutine(basicPathfinding.UpdatePath());
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, target.position) < meleeRange)
        {
            StartCoroutine(MeleeAttack());
        }
    }

    private IEnumerator MeleeAttack()
    {
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(2);
        playerStatManager.TakeDamage(attackDamage);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            // Makes sure to only spawn one coin
            if (gameObject.activeSelf)
            {
                gameObject.SetActive(false);
                objectPooler.SpawnFromPool("Coins", transform.position, transform.rotation);
                enemySpawner.EnemyDefeated();
            }
        }
    }
}
