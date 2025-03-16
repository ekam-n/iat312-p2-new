using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    [Tooltip("Damage dealt to the player upon collision.")]
    public float damage = 20f;
    [Tooltip("Layer mask representing the ground. BossProjectile will ignore collisions with this layer.")]
    public LayerMask groundLayer;

    void Awake()
    {
        // Ignore collisions between this projectile's layer and the ground layer.
        int groundLayerNum = LayerMask.NameToLayer("Ground");
        Physics2D.IgnoreLayerCollision(gameObject.layer, groundLayerNum, true);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // If the collided object has a PlayerHealth component, apply damage.
        PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }
        
        // Destroy the projectile after any collision (ground collisions are ignored).
        Destroy(gameObject);
    }
}
