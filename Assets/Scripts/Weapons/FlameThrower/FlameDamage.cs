using UnityEngine;

public class FlameDamage : MonoBehaviour
{
    // Damage applied per second by the flame.
    public float damagePerSecond = 50f;
    // Layer masks for enemy objects.
    public LayerMask enemyLayer;
    public LayerMask flyingEnemyLayer;

    // Pushback settings:
    [Tooltip("Force applied to push the enemy away while being hit by the flame.")]
    public float pushBackForce = 10f;

    void OnTriggerStay2D(Collider2D other)
    {
        // Check if the collided object is on the enemy or flying enemy layer.
        if ((((1 << other.gameObject.layer) & enemyLayer) != 0) ||
            (((1 << other.gameObject.layer) & flyingEnemyLayer) != 0))
        {
            // Try to get the EnemyBase component.
            EnemyBase enemy = other.GetComponent<EnemyBase>();
            if (enemy != null)
            {
                // Apply damage over time.
                enemy.TakeDamage(damagePerSecond * Time.deltaTime);

                // Calculate the pushback direction from the flame's position to the enemy.
                Vector2 pushDirection = ((Vector2)other.transform.position - (Vector2)transform.position).normalized;

                // Get the enemy's Rigidbody2D and apply a continuous force.
                Rigidbody2D enemyRb = other.GetComponent<Rigidbody2D>();
                if (enemyRb != null)
                {
                    // Apply force scaled by Time.deltaTime so it's applied continuously.
                    enemyRb.AddForce(pushDirection * pushBackForce * Time.deltaTime, ForceMode2D.Force);
                }
            }
        }
    }
}
