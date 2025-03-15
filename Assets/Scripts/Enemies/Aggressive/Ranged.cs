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

    public override void Patrol()
    {
        // Keep the enemy stationary.
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
    }

    new void Update()
    {
        // Toggle vision cone display when V is pressed (for debugging).
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
                RaycastHit2D hit = Physics2D.Raycast(transform.position, toPlayer.normalized, toPlayer.magnitude, obstacleMask);
                if (hit.collider == null)
                {
                    // Use default facing when not chasing.
                    Vector2 defaultFacing = new Vector2(faceLeftByDefault ? -1f : 1f, 0f);
                    float angleToPlayer = Vector2.Angle(defaultFacing, toPlayer);
                    isChasing = (angleToPlayer <= visionAngle);
                }
                else
                {
                    isChasing = false;
                }
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
        Patrol();
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            // Only flip sprite toward the player if chasing; otherwise, use the default.
            sr.flipX = isChasing && target != null ? (target.position.x < transform.position.x) : faceLeftByDefault;
        }

        // --- Ranged Attack ---
        if (isChasing)
        {
            // If we just started chasing, reset shootTimer so we fire immediately.
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
            // If not chasing, reset the timer.
            shootTimer = shootCooldown;
        }

        // Update previous chase state.
        wasChasing = isChasing;
    }

    void ShootProjectile()
    {
        if (projectilePrefab != null && shootPoint != null && target != null)
        {
            // Calculate the direction from shootPoint to the player.
            Vector2 direction = ((Vector2)target.position - (Vector2)shootPoint.position).normalized;
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
}
