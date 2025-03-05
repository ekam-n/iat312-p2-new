using UnityEngine;

public class FireballCollision : MonoBehaviour
{
    public float damage = 20f;          // Damage dealt by the fireball
    public LayerMask enemyLayer;        // Set this in the Inspector to the enemy layer(s)
    public LayerMask groundLayer;       // Set this in the Inspector to the ground layer(s)

    void Start()
    {
        // Destroy the fireball after 10 seconds if it doesn't hit anything.
        Destroy(gameObject, 10f);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collided object's layer is in the enemy layer mask.
        if (((1 << collision.gameObject.layer) & enemyLayer) != 0)
        {
            EnemyBase enemy = collision.gameObject.GetComponent<EnemyBase>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            Destroy(gameObject);
            return;
        }
        
        // Check if the collided object's layer is in the ground layer mask.
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            Destroy(gameObject);
        }
    }
}
