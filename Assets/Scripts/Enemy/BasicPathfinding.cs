using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BasicPathfinding : MonoBehaviour
{
    [Header("Stats")]
    public float speed = 5f;
    public EnemyType enemyType;

    [Header("Pathfinding")]
    public Transform target;
    public float stoppingDistance = 0.5f;
    public float detectionRadius = 1f;
    public float bufferDistance = 0.5f;
    public float pathUpdateInterval = 0.1f; // Interval in seconds to update the path

    private Vector3[] path;
    private int targetIndex;
    private Rigidbody2D rb;
    private PathRequestManager pathRequestManager;

    private void Start()
    {
        speed = enemyType.moveSpeed;

        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component not found on the Unit.");
            return;
        }

        pathRequestManager = FindObjectOfType<PathRequestManager>(); // Find Pathfinding script
        if (pathRequestManager == null)
        {
            Debug.LogError("Pathfinding is not assigned!");
            return;
        }

    }

    private void Update()
    {
        Debug.DrawRay(transform.position, transform.right * bufferDistance, Color.green);

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

    public IEnumerator UpdatePath()
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