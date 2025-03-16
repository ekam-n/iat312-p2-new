using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    [Tooltip("Damage dealt to the player upon collision.")]
    public float damage = 20f;
    [Tooltip("Layer mask representing the ground. BossProjectile will ignore collisions with this layer.")]
    public LayerMask groundLayer;

    void Awake()
    {
        // Get the ground layer number.
        int groundLayerNum = LayerMask.NameToLayer("Ground");
        // Now ignore collisions only between the BossProjectile layer and the ground.
        // Since this projectile is on the "BossProjectile" layer, this won't affect other enemy objects.
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
        
        // Destroy the projectile after any collision.
        Destroy(gameObject);
    }
}
