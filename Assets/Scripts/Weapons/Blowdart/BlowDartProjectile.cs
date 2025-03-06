using UnityEngine;

public class BlowDartProjectile : MonoBehaviour
{
    public float tranquilizeDuration = 15f;  // Duration the enemy stays asleep.
    public LayerMask enemyLayer;             // Set in the Inspector to the enemy layer.
    
    void Start()
    {
        // Self-destruct after 10 seconds if nothing is hit.
        Destroy(gameObject, 10f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collided object's layer is in the enemyLayer mask.
        if (((1 << other.gameObject.layer) & enemyLayer) != 0)
        {
            // Attempt to get an EnemyBase component.
            EnemyBase enemy = other.GetComponent<EnemyBase>();
            if (enemy != null)
            {
                enemy.Tranquilize(tranquilizeDuration);
            }
            Destroy(gameObject);
        }
        else
        {
            // Optionally, if the dart hits something else (like the ground), destroy it.
            Destroy(gameObject);
        }
    }
}
