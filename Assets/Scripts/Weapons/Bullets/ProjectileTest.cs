using UnityEngine;

public class ProjectileMotion : MonoBehaviour
{
    public Vector2 initialVelocity; // Initial velocity (magnitude and direction)
    public float gravity = 9.8f; // Gravity acceleration
    public float groundY = 0f; // The y position representing the ground level
    public LayerMask collisionLayer; // Layer for collision detection

    private Vector2 velocity; // Current velocity
    private Rigidbody2D rb; // Rigidbody2D component

    void Start()
    {
        // Initialize the velocity with the initial value
        velocity = initialVelocity;

        // Get the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Apply gravity to the velocity
        velocity.y -= gravity * Time.deltaTime;

        // Calculate the new position
        Vector2 newPosition = (Vector2)transform.position + velocity * Time.deltaTime;

        // Check if the projectile has hit the ground
        if (newPosition.y <= groundY)
        {
            newPosition.y = groundY;
            velocity = Vector2.zero;
        }

        // Apply the new position to the Rigidbody2D
        rb.MovePosition(newPosition);

        // Check for collisions
        CheckForCollision();
    }

    void CheckForCollision()
    {
        // Cast a ray in the direction of the velocity to detect collisions
        RaycastHit2D hit = Physics2D.Raycast(transform.position, velocity.normalized, velocity.magnitude * Time.deltaTime, collisionLayer);
        if (hit.collider != null)
        {
            // Stop the projectile and handle the collision
            velocity = Vector2.zero;

            // Optional: Handle what happens when a collision occurs
            OnCollision(hit.collider);
        }
    }

    void OnCollision(Collider2D collider)
    {
        // Handle the collision logic, such as destroying the projectile or dealing damage
        Debug.Log("Collision detected with " + collider.name);
    }
}
