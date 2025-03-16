using UnityEngine;
using System.Collections;

public class FlyingVertical : SimpleEnemy
{
    // Public flag to choose default horizontal facing when patrolling.
    public bool faceLeftByDefault = false;
    
    // In vertical patrol, patrolDirection represents vertical direction (1 for up, -1 for down).
    // initialPosition is used as the center of the vertical patrol range.
    
    protected override void Awake()
    {
        base.Awake();
        rb.gravityScale = 0f;  // Disable gravity for flying behavior.
        initialPosition = transform.position; // Store initial position (vertical patrol center).
    }
    
    new void Update()
    {
        // Toggle vision cone display when V is pressed.
        if (Input.GetKeyDown(KeyCode.V))
        {
            showVisionCone = !showVisionCone;
        }
        
        // Vision detection: use the default horizontal facing direction.
        if (target != null)
        {
            Vector2 toPlayer = target.position - transform.position;
            if (toPlayer.magnitude <= visionRange)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, toPlayer.normalized, toPlayer.magnitude, obstacleMask);
                if (hit.collider == null)
                {
                    // Use horizontal forward based on faceLeftByDefault.
                    Vector2 forward = faceLeftByDefault ? Vector2.left : Vector2.right;
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
            // In chase mode, fly directly toward the player with full 2D movement.
            if (target != null)
            {
                Vector2 direction = (target.position - transform.position).normalized;
                rb.linearVelocity = direction * moveSpeed * chaseSpeedMultiplier;
                // Flip sprite based on horizontal movement (original behavior).
                SpriteRenderer sr = GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    float directionX = Mathf.Sign(target.position.x - transform.position.x);
                    sr.flipX = (directionX < 0);
                }
            }
            else
            {
                rb.linearVelocity = Vector2.zero;
            }
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0f)
            {
                PerformAttack();
                attackTimer = attackCooldown;
            }
        }
        else
        {
            // In patrol mode, fly vertically.
            Patrol();
            attackTimer = attackCooldown;
        }
    }
    
    public override void Patrol()
    {
        if (target != null)
        {
            rb.linearVelocity = new Vector2(0, patrolDirection * moveSpeed);
            // Define vertical patrol bounds.
            float lowerBound = initialPosition.y - patrolRange;
            float upperBound = initialPosition.y + patrolRange;
            if (transform.position.y >= upperBound && patrolDirection > 0)
            {
                patrolDirection = -1;
            }
            else if (transform.position.y <= lowerBound && patrolDirection < 0)
            {
                patrolDirection = 1;
            }
            // Lock horizontal position.
            transform.position = new Vector3(initialPosition.x, transform.position.y, transform.position.z);
            
            // Use the public flag to set default horizontal facing.
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.flipX = faceLeftByDefault;
            }
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
    
    // Draw the vision cone gizmo matching the actual vision cone.
    void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            return;
        if (showVisionCone)
        {
            Gizmos.color = Color.yellow;
            Vector3 startPos = transform.position;
            // Use default horizontal facing direction for the vision cone.
            Vector2 forward = faceLeftByDefault ? Vector2.left : Vector2.right;
            // Calculate cone boundaries.
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
