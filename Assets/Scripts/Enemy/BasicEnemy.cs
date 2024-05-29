using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BasicEnemy : MonoBehaviour, IPooledObject
{
    public Transform target;
    public float speed = 5f;
    public float stoppingDistance = 0.5f;
    public float detectionRadius;
    public EnemyType basicEnemy;
    public float bufferDistance;
    public float pathUpdateInterval = 0.1f; // Interval in seconds to update the path

    private Vector3[] path;
    private int targetIndex;
    private Rigidbody2D rb;
    private float health;
    private float damage;
    private PathRequestManager pathRequestManager;
    private ObjectPooler objectPooler;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
    }

    public void OnObjectSpawn()
    {
        health = basicEnemy.health;
        damage = basicEnemy.damage;
        target = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(UpdatePath());
    }

    private void FixedUpdate()
    {
        if (path != null && path.Length > 0 && targetIndex < path.Length)
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

    private void Update()
    {
        Debug.DrawRay(transform.position, transform.right * bufferDistance, Color.green);
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, detectionRadius, transform.right, 0, LayerMask.GetMask("Player"));
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