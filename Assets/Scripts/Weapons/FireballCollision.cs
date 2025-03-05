using UnityEngine;

public class FireballCollision : MonoBehaviour
{
    public LayerMask groundLayer; // Set this in the Inspector to the ground layer(s)

    void Start()
    {
        // Destroy the fireball after 10 seconds if nothing else happens.
        Destroy(gameObject, 10f);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collided object's layer is in the groundLayer mask.
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            Destroy(gameObject);
        }
    }
}
