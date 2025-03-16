using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [Tooltip("Damage dealt to the player upon collision.")]
    public float damage = 20f;
    [Tooltip("Layer mask representing the ground.")]
    public LayerMask groundLayer;

    void OnCollisionEnter2D(Collision2D collision)
    {
        // First, check if the collided object is on the ground.
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            Destroy(gameObject);
            return;
        }
        
        // If the collided object has a PlayerHealth component, apply damage.
        PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }
        
        // Destroy the projectile after any collision.
        Destroy(gameObject);
    }
}
