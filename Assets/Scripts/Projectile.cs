using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 5f;
    public float paralysisDuration = 3f;
    private Vector2 direction;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0; // Prevent gravity from affecting the projectile
        Destroy(gameObject, lifetime);
    }

    void FixedUpdate()
    {
        rb.linearVelocity = direction * speed; // Move in the assigned direction
    }

    // Method to set the direction of the projectile
    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized; // Ensure unit direction
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyController enemy = collision.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.Paralyze(paralysisDuration);
            }
            Destroy(gameObject);
        }
    }
}
