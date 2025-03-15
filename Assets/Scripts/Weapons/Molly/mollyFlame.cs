using UnityEngine;

public class mollyFlame : MonoBehaviour
{
    // Damage applied per second by the flame.
    public float damagePerSecond = 50f;
    // Layer mask for enemy objects.
    public LayerMask enemyLayer;
    public LayerMask flyingEnemyLayer;       // Set this in the Inspector to the ground layer(s)

    // Slow effect settings:
    [Tooltip("Duration of the slow effect in seconds.")]
    public float slowDuration = 3f;
    [Tooltip("Multiplier applied to enemy speed (e.g., 0.5 for 50% of original).")]
    public float slowMultiplier = 0.5f;
    void Start()
    {
        // Destroy the flame object after 5 seconds
        Destroy(gameObject, 5f);
    }
    void OnTriggerStay2D(Collider2D other)
    {
        // Check if the collided object is on the enemy layer.
        if ((((1 << other.gameObject.layer) & enemyLayer) != 0) || ((1 << other.gameObject.layer) & flyingEnemyLayer) != 0)
        {
            EnemyBase enemy = other.GetComponent<EnemyBase>();
            if (enemy != null)
            {
                // Apply damage over time.
                enemy.TakeDamage(damagePerSecond * Time.deltaTime);

                // Apply or reset the slow effect.
                SlowEffect slowEffect = other.GetComponent<SlowEffect>();
                if (slowEffect == null)
                {
                    slowEffect = other.gameObject.AddComponent<SlowEffect>();
                    slowEffect.slowDuration = slowDuration;
                    slowEffect.slowMultiplier = slowMultiplier;
                }
                else
                {
                    slowEffect.ResetDuration();
                }
            }
        }
    }
}
