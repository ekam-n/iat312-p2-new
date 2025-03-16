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
                // Cast a ray toward the player.
                RaycastHit2D hit = Physics2D.Raycast(transform.position, toPlayer.normalized, toPlayer.magnitude, obstacleMask);
                if (hit.collider == null)
                {
                    // When not chasing, use the default facing based on faceLeftByDefault.
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
        Patrol(); // (enemy is stationary)

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            // If chasing, face toward the player; otherwise, use default based on faceLeftByDefault.
            if (isChasing && target != null)
            {
                sr.flipX = (target.position.x < transform.position.x);
            }
            else
            {
                sr.flipX = faceLeftByDefault;
            }
        }

        // --- Adjust ShootPoint Position ---
        if (shootPoint != null && sr != null)
        {
            // When the enemy's sprite is flipped (i.e. facing left) the shootPoint's x should be flipped.
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
            // When the enemy first starts chasing, fire immediately.
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
            // When not chasing, reset the shoot timer.
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
}
