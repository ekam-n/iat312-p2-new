using UnityEngine;

public class FireballCollision : MonoBehaviour
{
    // Set this in the Inspector to the layer mask representing the ground.
    public LayerMask groundLayer;

    // This method is called when a collision happens (make sure the collider is NOT set as a trigger).
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collided object is in the ground layer.
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            Destroy(gameObject);
        }
    }
}
