using UnityEngine;
using System.Collections;

public class RangedEnemy : StationaryEnemy
{
    [Header("Ranged Attack Settings")]
    [Tooltip("The projectile prefab that will be fired at the player.")]
    public GameObject projectilePrefab;
    [Tooltip("The transform from which the projectile will be spawned.")]
    public Transform shootPoint;
    [Tooltip("Time (in seconds) between consecutive shots.")]
    public float shootCooldown = 2f;
    [Tooltip("Speed at which the projectile is fired.")]
    public float projectileSpeed = 10f;

    // Timer to control shooting frequency.
    private float shootTimer = 0f;
    // Track the previous chase state.
    private bool wasChasing = false;

    // Store the original local position of the shoot point.
    private Vector3 originalShootPointLocalPos;

    protected override void Awake()
    {
        base.Awake();
        // Store the shootPoint's original local position.
        if (shootPoint != null)
        {
            originalShootPointLocalPos = shootPoint.localPosition;
        }


    }

    public override void Patrol()
    {
        // For a stationary enemy, simply remain in place.
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
    }

    new void Update()
    {
        // (Optional) Toggle vision cone display for debugging.
        if (Input.GetKeyDown(KeyCode.V))
        {
            showVisionCone = !showVisionCone;
        }

        // --- Vision Detection ---
        if (target != null)
        {
            Vector2 toPlayer = target.position - transform.position;
            if (toPlayer.magnitude <= visionRange)
            {
                // Cast a ray toward the player to see if an obstacle blocks view.
                RaycastHit2D hit = Physics2D.Raycast(transform.position, toPlayer.normalized, toPlayer.magnitude, obstacleMask);
                // If nothing blocks view, the enemy sees the player.
                isChasing = (hit.collider == null);
            }
            else
            {
                isChasing = false;
            }
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }

        // --- Movement & Facing ---
        // Since this enemy is stationary, we just call Patrol() to keep it in place.
        Patrol();

        // Get the SpriteRenderer so we can flip the sprite.
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.flipX = faceLeftByDefault;

        // --- Adjust ShootPoint Position ---
        if (shootPoint != null && sr != null)
        {
            // When the enemy is facing left (flipX true), flip the shoot point’s x value.
            if (sr.flipX)
            {
                shootPoint.localPosition = new Vector3(-Mathf.Abs(originalShootPointLocalPos.x), originalShootPointLocalPos.y, originalShootPointLocalPos.z);
            }
            else
            {
                shootPoint.localPosition = originalShootPointLocalPos;
            }
        }

        // --- Ranged Attack ---
        if (isChasing)
        {
            // If the enemy just started chasing, fire immediately.
            if (!wasChasing)
            {
                shootTimer = 0f;
            }
            shootTimer -= Time.deltaTime;
            if (shootTimer <= 0f)
            {
                ShootProjectile();
                shootTimer = shootCooldown;
            }
        }
        else
        {
            // If not chasing, reset the shoot timer.
            shootTimer = shootCooldown;
        }

        wasChasing = isChasing;
    }

    void ShootProjectile()
    {
        if (projectilePrefab != null && shootPoint != null && target != null)
        {
            // Calculate the normalized direction from the shoot point to the player.
            Vector2 direction = (((Vector2)target.position) - ((Vector2)shootPoint.position)).normalized;
            // Compute the angle (in degrees) for the projectile so it faces the player.
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            // Instantiate the projectile with the proper rotation.
            GameObject proj = Instantiate(projectilePrefab, shootPoint.position, Quaternion.Euler(0, 0, angle));
            Rigidbody2D projRb = proj.GetComponent<Rigidbody2D>();
            if (projRb != null)
            {
                projRb.linearVelocity = direction * projectileSpeed;
            }
        }
    }
}
