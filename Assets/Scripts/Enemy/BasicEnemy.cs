using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BasicEnemy : MonoBehaviour, IPooledObject
{
    [Header("Stats")]
    public float speed = 5f;
    public EnemyType basicEnemy;

    [Header("Pathfinding")]
    public Transform target;
    public float stoppingDistance = 0.5f;
    public float detectionRadius;
    public float bufferDistance;
    public float pathUpdateInterval = 0.1f; // Interval in seconds to update the path

    [Header("Self Destruct")]
    [Tooltip("Distance from player before enemy begins self-destruct sequence")]
    public float selfDestructRange;
    [Tooltip("Amount of time it takes to blow up (Should match animation time)")]
    public float selfDestructTime;
    [Tooltip("Damage done to player if player is in range of explosion")]
    public int selfDestructDamage;

    private Vector3[] path;
    private int targetIndex;
    private Rigidbody2D rb;
    private float health;
    private float damage;
    private PathRequestManager pathRequestManager;
    private ObjectPooler objectPooler;
    private PlayerStatManager playerStatManager;
    private EnemySpawner enemySpawner;

    private Coroutine selfDestructCoroutine;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemySpawner = GameObject.Find("EnemySpawner").GetComponent<EnemySpawner>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component not found on the Unit.");
            return;
        }

        objectPooler = ObjectPooler.Instance;
        pathRequestManager = FindObjectOfType<PathRequestManager>(); // Find Pathfinding script
        if (pathRequestManager == null)
        {
            Debug.LogError("Pathfinding is not assigned!");
            return;
        }

        playerStatManager = GameObject.Find("Player").GetComponent<PlayerStatManager>();
    }

    public void OnObjectSpawn()
    {
        health = basicEnemy.health;
        damage = basicEnemy.damage;
        target = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(UpdatePath());
    }

    private void Update()
    {
        Debug.DrawRay(transform.position, transform.right * bufferDistance, Color.green);
        if (selfDestructCoroutine == null && Vector2.Distance(transform.position, target.position) < selfDestructRange)
        {
            selfDestructCoroutine = StartCoroutine(SelfDestruct());
        }
    }

    private void FixedUpdate()
    {
        if (path != null && path.Length > 0 && targetIndex < path.Length && selfDestructCoroutine == null)
        {
            Vector3 currentWaypoint = path[targetIndex];
            if (Vector2.Distance(transform.position, currentWaypoint) < stoppingDistance)
            {
                targetIndex++;
                if (targetIndex >= path.Length)
                {
                    rb.velocity = Vector2.zero;
                    return;
                }
            }

            Vector2 direction = (currentWaypoint - transform.position).normalized;
            rb.velocity = direction * speed;

            // Rotate to face the direction of movement
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            // Check if close to the player to stop
            if (Vector2.Distance(target.position, transform.position) < bufferDistance)
            {
                rb.velocity = Vector2.zero;
            }
        }
    }



    public void TakeDamage(float damageTaken)
    {
        health -= damageTaken;
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

    private IEnumerator UpdatePath()
    {
        while (true)
        {
            RequestPath();
            yield return new WaitForSeconds(pathUpdateInterval);
        }
    }

    private IEnumerator SelfDestruct()
    {
        rb.velocity = Vector2.zero;
        // Play animation
        yield return new WaitForSeconds(selfDestructTime);
        // Damage player
        if (Vector2.Distance (transform.position, target.position) <= selfDestructRange)
        {
            playerStatManager.TakeDamage(selfDestructDamage);
        }
        gameObject.SetActive(false);
        //Spawn explosion effect
    }

    private void RequestPath()
    {
        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
    }

    private void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful && newPath.Length > 0)
        {
            path = newPath;
            targetIndex = 0; // Reset the targetIndex to 0
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.DrawRay(transform.position, transform.right * detectionRadius);
    }
}