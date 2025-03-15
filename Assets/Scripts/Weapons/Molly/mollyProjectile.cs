using UnityEngine;

public class mollyProjectile : MonoBehaviour
{
    public GameObject flamePrefab; // Assign the flame effect prefab in the Inspector
    public LayerMask groundLayer; // Layer for ground detection

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the projectile hit the ground
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            Explode();
        }
    }

    void Explode()
    {
        // Spawn the flame effect at the impact position
        if (flamePrefab != null)
        {
            Instantiate(flamePrefab, transform.position, Quaternion.identity);
        }

        // Destroy the molotov object
        Destroy(gameObject);
    }
}
