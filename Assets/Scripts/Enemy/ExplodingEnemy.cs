using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingEnemy : MonoBehaviour, IPooledObject, IDamageable
{
    public EnemyType basicEnemy;

    public float health;
    public float speed;
    public float attackDamage;
    public float spawnWeight;

    [Header("Self Destruct")]
    [Tooltip("Distance from player before enemy begins self-destruct sequence")]
    public float selfDestructRange;
    [Tooltip("Amount of time it takes to blow up (Should match animation time)")]
    public float selfDestructTime;
    [Tooltip("Damage done to player if player is in range of explosion")]
    public int selfDestructDamage;

    private Coroutine selfDestructCoroutine;
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

        health = basicEnemy.health;
        speed = basicEnemy.moveSpeed;
        attackDamage = basicEnemy.damage;

        target = GameObject.FindGameObjectWithTag("Player").transform;
        basicPathfinding.target = target;
        StartCoroutine(basicPathfinding.UpdatePath());
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, transform.right * selfDestructRange, Color.green);
        if (selfDestructCoroutine == null && Vector2.Distance(transform.position, target.position) < selfDestructRange)
        {
            Debug.Log("Got into the if loop");
            selfDestructCoroutine = StartCoroutine(SelfDestruct());
        }
    }

    private IEnumerator SelfDestruct()
    {
        rb.velocity = Vector2.zero;
        // Play animation
        yield return new WaitForSeconds(selfDestructTime);
        // Damage player
        if (Vector2.Distance(transform.position, target.position) <= selfDestructRange)
        {
            playerStatManager.TakeDamage(selfDestructDamage);
        }
        enemySpawner.EnemyDefeated();
        gameObject.SetActive(false);
        //Spawn explosion effect
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
