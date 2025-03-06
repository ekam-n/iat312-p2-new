using UnityEngine;

public class FlameDamage : MonoBehaviour
{
    // Damage applied per second by the flame.
    public float damagePerSecond = 50f;

    // Layer mask for enemy objects.
    public LayerMask enemyLayer;

    void OnTriggerStay2D(Collider2D other)
    {
        // Check if the collided object is on the enemy layer.
        if (((1 << other.gameObject.layer) & enemyLayer) != 0)
        {
            // Try to get the EnemyBase component from the collided object.
            EnemyBase enemy = other.GetComponent<EnemyBase>();
            if (enemy != null)
            {
                // Apply damage over time.
                enemy.TakeDamage(damagePerSecond * Time.deltaTime);
            }
        }
    }
}
