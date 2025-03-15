using UnityEngine;
using System.Collections;

public class FlyingStationary : StationaryEnemy
{
    [Header("Flying Settings")]
    [Tooltip("Speed at which the enemy returns to its initial position (x and y) when not chasing.")]
    public float returnSpeedFlying = 2f;
    [Tooltip("Angle offset (in degrees) to adjust the default vision cone direction.")]
    public float visionOffset = 0f; // Positive rotates clockwise

    protected override void Awake()
    {
        base.Awake();
        // Disable gravity for flying behavior.
        rb.gravityScale = 0f;
        // For flying enemies, consider the full initial position (x,y) as home.
        initialPosition = transform.position;

        // Set the initial sprite flip based on the inspector value.
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.flipX = faceLeftByDefault;
        }
    }

    new void Update()
    {
        // Toggle vision cone display when V is pressed.
        if (Input.GetKeyDown(KeyCode.V))
        {
            showVisionCone = !showVisionCone;
        }

        // Vision detection
        if (target != null)
        {
            Vector2 toPlayer = target.position - transform.position;
            if (toPlayer.magnitude <= visionRange)
            {
                // Cast a ray toward the player.
                RaycastHit2D hit = Physics2D.Raycast(transform.position, toPlayer.normalized, toPlayer.magnitude, obstacleMask);
                if (hit.collider == null)
                {
                    // When not chasing, use default facing based on faceLeftByDefault.
                    Vector2 defaultFacing = new Vector2(faceLeftByDefault ? -1f : 1f, 0f);
                    // Apply the vision offset.
                    defaultFacing = Quaternion.Euler(0, 0, visionOffset) * defaultFacing;
                    // When chasing, use the actual direction toward the player.
                    Vector2 forward = isChasing ? toPlayer.normalized : defaultFacing;
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
            rb.linearVelocity = Vector2.zero;
        }

        if (isChasing)
        {
            // In chase mode, fly directly toward the player (full 2D movement).
            if (target != null)
            {
                Vector2 direction = (target.position - transform.position).normalized;
                rb.linearVelocity = direction * moveSpeed * chaseSpeedMultiplier;
                // Flip sprite based on the horizontal component.
                SpriteRenderer sr = GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    sr.flipX = (direction.x < 0);
                }
            }
            else
            {
                rb.linearVelocity = Vector2.zero;
            }
            attackTimer -= Time.deltaTime;
        }
        else
        {
            // Not chasing: smoothly return to the initial (x,y) position.
            float newX = Mathf.MoveTowards(transform.position.x, initialPosition.x, returnSpeedFlying * Time.deltaTime);
            float newY = Mathf.MoveTowards(transform.position.y, initialPosition.y, returnSpeedFlying * Time.deltaTime);
            transform.position = new Vector3(newX, newY, transform.position.z);
            rb.linearVelocity = Vector2.zero;
            attackTimer = attackCooldown;
        }
    }

    new void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            return;
        if (showVisionCone)
        {
            Gizmos.color = Color.yellow;
            Vector3 startPos = transform.position;
            // When not chasing, use the default facing with vision offset.
            Vector2 defaultFacing = new Vector2(faceLeftByDefault ? -1f : 1f, 0f);
            defaultFacing = Quaternion.Euler(0, 0, visionOffset) * defaultFacing;
            Vector2 forward = !isChasing ? defaultFacing : new Vector2(Mathf.Sign(target.position.x - transform.position.x), 0f);
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
