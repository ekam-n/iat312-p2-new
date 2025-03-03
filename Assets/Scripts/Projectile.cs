using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 200f; // Increased speed
    public float lifetime = 5f; // Time before the projectile is destroyed
    public float paralysisDuration = 3f; // How long enemies are paralyzed
    private Vector2 direction; // Direction of projectile movement
    private Rigidbody2D rb; // Reference to Rigidbody2D component

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0; // Prevent gravity from affecting the projectile
        Destroy(gameObject, lifetime); // Destroy the projectile after a set time
    }

    void FixedUpdate()
    {
        // Apply velocity in the direction the projectile is moving
        rb.linearVelocity = direction * 3*speed;
        Debug.Log("Projectile Speed: " + rb.linearVelocity.magnitude); // Debug speed
    }

    // Method to set the direction of the projectile
    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized; // Normalize to ensure uniform speed
        rb.linearVelocity = direction * speed; // Apply velocity immediately

        // Rotate the projectile to face the direction of movement
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyController enemy = collision.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.Paralyze(paralysisDuration); // Apply paralysis to the enemy
            }
            Destroy(gameObject); // Destroy the projectile on impact with an enemy
        }
    }
}