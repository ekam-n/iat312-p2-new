using UnityEngine;
using System.Collections;

public class StationaryEnemy : SimpleEnemy
{
    [Header("Stationary Settings")]
    [Tooltip("If true, the enemy will face left while stationary if no movement is occurring; otherwise it faces right.")]
    public bool faceLeftByDefault = false;
    [Tooltip("Speed at which the enemy returns to its initial horizontal position when not chasing.")]
    public float returnSpeed = 10f;

    // Override Patrol so that instead of moving along a patrol route, the enemy returns to its initial horizontal position.
    public override void Patrol()
    {
        // Smoothly move the enemy's x position toward its initial x position,
        // but keep the current y position so gravity continues to affect it.
        float newX = Mathf.MoveTowards(transform.position.x, initialPosition.x, returnSpeed * Time.deltaTime);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);

        // Zero out horizontal velocity (we're manually positioning the enemy).
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

        // Determine movement direction: if the enemy's current x is less than initial, it needs to move right; if greater, then left.
        float moveDir = initialPosition.x - transform.position.x;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            if (moveDir > 0f)
            {
                // Need to move right → face right (flipX false)
                sr.flipX = false;
            }
            else if (moveDir < 0f)
            {
                // Need to move left → face left (flipX true)
                sr.flipX = true;
            }
            else
            {
                // No movement, use default setting.
                sr.flipX = faceLeftByDefault;
            }
        }
    }

    new void Update()
    {
        // Toggle vision cone display when V is pressed.
        if (Input.GetKeyDown(KeyCode.V))
        {
            showVisionCone = !showVisionCone;
        }

        // Vision detection (same as before).
        if (target != null)
        {
            Vector2 toPlayer = target.position - transform.position;
            if (toPlayer.magnitude <= visionRange)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, toPlayer.normalized, toPlayer.magnitude, obstacleMask);
                if (hit.collider == null)
                {
                    // When not chasing, use the default facing direction based on our movement.
                    Vector2 forward = isChasing 
                        ? new Vector2(Mathf.Sign(target.position.x - transform.position.x), 0)
                        : new Vector2(GetDefaultFacing(), 0f);
                    float angleToPlayer = Vector2.Angle(forward, toPlayer);
                    if (angleToPlayer <= visionAngle)
                    {
                        isChasing = true;
                        lostSightTimer = 0f;
                    }
                    else if (isChasing)
                    {
                        lostSightTimer += Time.deltaTime;
                        if (lostSightTimer >= lostSightTimeThreshold)
                        {
                            isChasing = false;
                            lostSightTimer = 0f;
                        }
                    }
                }
                else
                {
                    if (isChasing)
                    {
                        lostSightTimer += Time.deltaTime;
                        if (lostSightTimer >= lostSightTimeThreshold)
                        {
                            isChasing = false;
                            lostSightTimer = 0f;
                        }
                    }
                }
            }
            else
            {
                if (isChasing)
                {
                    lostSightTimer += Time.deltaTime;
                    if (lostSightTimer >= lostSightTimeThreshold)
                    {
                        isChasing = false;
                        lostSightTimer = 0f;
                    }
                }
            }
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }

        if (isChasing)
        {
            if (target != null)
            {
                float directionX = Mathf.Sign(target.position.x - transform.position.x);
                rb.linearVelocity = new Vector2(directionX * moveSpeed * chaseSpeedMultiplier, rb.linearVelocity.y);
                SpriteRenderer sr = GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    sr.flipX = (directionX < 0);
                }
            }
            else
            {
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            }
            attackTimer -= Time.deltaTime;
        }
        else
        {
            // If magnetized (i.e. target is the coconut), skip Patrol to allow magnetization to control positioning.
            if (target != null && target.GetComponent<coconutProjectile>() != null)
            {
                // Do nothing or simply hold the current position.
                attackTimer = attackCooldown;
            }
            else
            {
                // Normal patrol behavior: return to the initial horizontal position.
                Patrol();
                attackTimer = attackCooldown;
            }
        }
    }

    // Helper method to determine the default facing direction based on current movement.
    // Returns 1 for right, -1 for left.
    private float GetDefaultFacing()
    {
        float moveDir = initialPosition.x - transform.position.x;
        if (moveDir > 0f)
            return 1f; // Moving right.
        else if (moveDir < 0f)
            return -1f; // Moving left.
        else
            return faceLeftByDefault ? -1f : 1f;
    }

    // Override OnDrawGizmos to ensure the vision cone is drawn correctly.
    new void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            return;
        if (showVisionCone)
        {
            Gizmos.color = Color.yellow;
            Vector3 startPos = transform.position;
            Vector2 forward = !isChasing 
                ? new Vector2(GetDefaultFacing(), 0f)
                : new Vector2(Mathf.Sign(target.position.x - transform.position.x), 0f);
            Vector2 leftBoundary = Quaternion.Euler(0, 0, -visionAngle) * forward;
            Vector2 rightBoundary = Quaternion.Euler(0, 0, visionAngle) * forward;
            Gizmos.DrawLine(startPos, startPos + (Vector3)(leftBoundary.normalized * visionRange));
            Gizmos.DrawLine(startPos, startPos + (Vector3)(rightBoundary.normalized * visionRange));

            int segments = 20;
            Vector3 previousPoint = startPos + (Vector3)(leftBoundary.normalized * visionRange);
            float totalAngle = visionAngle * 2;
            for (int i = 1; i <= segments; i++)
            {
                float stepAngle = -visionAngle + (totalAngle / segments) * i;
                Vector2 stepDir = Quaternion.Euler(0, 0, stepAngle) * forward;
                Vector3 nextPoint = startPos + (Vector3)(stepDir.normalized * visionRange);
                Gizmos.DrawLine(previousPoint, nextPoint);
                previousPoint = nextPoint;
            }
        }
    }
}
