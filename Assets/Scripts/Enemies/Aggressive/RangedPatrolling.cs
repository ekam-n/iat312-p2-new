using UnityEngine;
using System.Collections;

public class RangedPatrolling : SimpleEnemy
{
    [Header("Ranged Attack Settings")]
    [Tooltip("The projectile prefab that will be fired at the player.")]
    public GameObject projectilePrefab;
    [Tooltip("The transform from which the projectile will be spawned.")]
    public Transform shootPoint;
    [Tooltip("Time (in seconds) between consecutive shots after the first shot.")]
    public float shootCooldown = 2f;
    [Tooltip("Speed at which the projectile is fired.")]
    public float projectileSpeed = 10f;

    // Timer to control shooting frequency.
    private float shootTimer = 0f;
    // Flag to indicate that the enemy is in 'shooting' mode (player detected).
    private bool isShooting = false;
    private bool wasChasing = false;

    // Store the original local position of the shoot point.
    private Vector3 originalShootPointLocalPos;

    protected override void Awake()
    {
        base.Awake();
        // Store the shootPoint’s original local position.
        if (shootPoint != null)
        {
            originalShootPointLocalPos = shootPoint.localPosition;
        }
    }

    new void Update()
    {
        // (Optional) Toggle vision cone display for debugging.
        if (Input.GetKeyDown(KeyCode.V))
        {
            showVisionCone = !showVisionCone;
        }

        // --- Vision Detection ---
        // If the player (target) exists, check if it is within vision range and unobstructed.
        if (target != null)
        {
            Vector2 toPlayer = target.position - transform.position;
            if (toPlayer.magnitude <= visionRange)
            {
                // Cast a ray from the enemy to the player.
                RaycastHit2D hit = Physics2D.Raycast(transform.position, toPlayer.normalized, toPlayer.magnitude, obstacleMask);
                // If no obstacle is hit and the player falls within the vision cone...
                if (hit.collider == null)
                {
                    // For vision, we use the enemy's current horizontal facing as defined by its patrol direction.
                    Vector2 forward = new Vector2(patrolDirection, 0);
                    float angleToPlayer = Vector2.Angle(forward, toPlayer);
                    // If the angle is within the set visionAngle, the enemy detects the player.
                    isShooting = (angleToPlayer <= visionAngle);
                }
                else
                {
                    // If an obstacle is blocking view, the enemy no longer sees the player.
                    isShooting = false;
                }
            }
            else
            {
                isShooting = false;
            }
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            isShooting = false;
        }

        // --- Movement & Facing ---
        // When not shooting, resume patrolling.
        if (!isShooting)
        {
            // Call the inherited Patrol() which makes the enemy move horizontally within its patrol range.
            Patrol();
            // Reset shoot timer so that a new detection fires immediately.
            shootTimer = shootCooldown;
        }
        else
        {
            // When shooting, stop all horizontal movement (but leave vertical velocity so gravity still applies).
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

            if (!wasChasing)
            {
                // If the enemy was not chasing before, reset the shoot timer to fire immediately.
                shootTimer = 0f;
            }

            // Handle shooting cooldown.
            shootTimer -= Time.deltaTime;
            if (shootTimer <= 0f)
            {
                ShootProjectile();
                shootTimer = shootCooldown;
            }
        }

        // --- Adjust ShootPoint Position ---
        // Use the same approach as in the Flamethrower script: store the original local position and flip its x value if needed.
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (shootPoint != null && sr != null)
        {
            // We flip the shoot point based on the enemy's sprite flip.
            if (sr.flipX)
            {
                shootPoint.localPosition = new Vector3(-Mathf.Abs(originalShootPointLocalPos.x), originalShootPointLocalPos.y, originalShootPointLocalPos.z);
            }
            else
            {
                shootPoint.localPosition = originalShootPointLocalPos;
            }
        }
        wasChasing = isShooting;
    }

    void ShootProjectile()
    {
        if (projectilePrefab != null && shootPoint != null && target != null)
        {
            // Calculate the normalized direction from the shoot point to the player.
            Vector2 direction = (((Vector2)target.position) - ((Vector2)shootPoint.position)).normalized;
            // Compute the angle (in degrees) for the projectile so that it faces the player.
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            // Instantiate the projectile with the computed rotation.
            GameObject proj = Instantiate(projectilePrefab, shootPoint.position, Quaternion.Euler(0, 0, angle));
            Rigidbody2D projRb = proj.GetComponent<Rigidbody2D>();
            if (projRb != null)
            {
                projRb.linearVelocity = direction * projectileSpeed;
            }
        }
    }

    // (Optional) Override OnDrawGizmos() if you want to see the vision cone for debugging.
}
