using UnityEngine;

public class BlowDartProjectile : MonoBehaviour
{
    public float tranquilizeDuration = 15f;  // Duration the enemy stays asleep.
    public LayerMask enemyLayer;             // Set in the Inspector to the enemy layer.
    public LayerMask flyingEnemyLayer;             // Set in the Inspector to the flyingEnemy layer.
    public LayerMask groundLayer;            // Set in the Inspector to the ground layer.
    private Rigidbody2D rb;
    private bool hasStuck = false;           // Flag to indicate if the dart has stuck to the ground.

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Self-destruct after 10 seconds if nothing is hit.
        Destroy(gameObject, 10f);
    }

    void FixedUpdate()
    {
        // Only update rotation if there is significant velocity.
        if (rb.linearVelocity.sqrMagnitude > 0.01f)
        {
            float angle = Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * Mathf.Rad2Deg;
            // If your sprite’s default orientation isn’t pointing to the right,
            // add an offset (e.g., 90 degrees) as needed.
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // If we've already stuck, do nothing.
        if (hasStuck)
            return;

        // Optional: Ignore collisions with the player if necessary.
        if (other.CompareTag("Player"))
            return;

        // Check for enemy collision.
        if ((((1 << other.gameObject.layer) & enemyLayer) != 0) || (((1 << other.gameObject.layer) & flyingEnemyLayer) != 0))
        {
            EnemyBase enemy = other.GetComponent<EnemyBase>();
            if (enemy != null)
            {
                enemy.Tranquilize(tranquilizeDuration);
            }
            Destroy(gameObject);
            return;
        }
        // Check for ground collision.
        else if (((1 << other.gameObject.layer) & groundLayer) != 0)
        {
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.simulated = false;
            }
            Collider2D col = GetComponent<Collider2D>();
            if (col != null)
            {
                col.enabled = false;
            }
            hasStuck = true;
            return;
        }
        else
        {
            // For any other collision, decide whether you want to ignore it or destroy the dart.
            // For example, you might log the collision and do nothing:
            Debug.Log("Dart collided with " + other.gameObject.name + " (ignored)");
            // Or, if you really want to destroy it, leave it as is:
            // Destroy(gameObject);
        }
    }

}
