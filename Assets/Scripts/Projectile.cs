using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f; // Speed of the projectile
    public float lifetime = 5f; // Time before the projectile is destroyed
    public float paralysisDuration = 3f; // Duration of paralysis
    private int direction = 1; // Default direction (right)
    private Rigidbody2D rb;

    void Start()
    {
        // Set up Rigidbody2D and ensure no gravity affects the projectile
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0; // Prevent gravity from affecting the projectile

        // Destroy the projectile after a set lifetime
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Move the projectile in the specified direction using Rigidbody2D
        rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y); // Only move horizontally
    }

    // Method to set the direction of the projectile (1 for right, -1 for left)
    public void SetDirection(int direction)
    {
        this.direction = direction;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the projectile collides with an enemy
        if (collision.CompareTag("Enemy"))
        {
            // Paralyze the enemy
            EnemyController enemy = collision.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.Paralyze(paralysisDuration);
            }

            // Destroy the projectile
            Destroy(gameObject);
        }
    }
}
