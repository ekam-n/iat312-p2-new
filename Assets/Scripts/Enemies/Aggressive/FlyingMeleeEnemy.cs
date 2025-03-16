using UnityEngine;
using System.Collections;

public class FlyingMeleeEnemy : SimpleEnemy
{
    // Override Awake to disable gravity and store the flying altitude.
    protected override void Awake()
    {
        base.Awake();
        rb.gravityScale = 0f;  // Disable gravity.
        initialPosition = transform.position; // Use starting position as flying altitude.
    }

    // Hide the base Update() with a new one.
    new void Update()
    {
        // Toggle vision cone display when V is pressed.
        if (Input.GetKeyDown(KeyCode.V))
        {
            showVisionCone = !showVisionCone;
        }

        // Vision check with obstacle blocking.
        if (target != null)
        {
            Vector2 toPlayer = target.position - transform.position;
            if (toPlayer.magnitude <= visionRange)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, toPlayer.normalized, toPlayer.magnitude, obstacleMask);
                if (hit.collider == null)
                {
                    // No obstacle blocking view; now check vision cone.
                    Vector2 forward = new Vector2(patrolDirection, 0); // Enemy's forward in patrol mode.
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

                // Flip sprite based on horizontal movement.
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

            if (attackTimer <= 0f)
            {
                PerformAttack();
                attackTimer = attackCooldown; // Reset cooldown after attacking
            }
        }
        else
        {
            // In patrol mode, fly left/right at a fixed altitude.
            Patrol();
            attackTimer = attackCooldown;
        }
    }

    // Override Patrol so that the enemy maintains its flying altitude.
    public override void Patrol()
    {
        if (target != null)
        {
            // Move only in the x direction.
            rb.linearVelocity = new Vector2(patrolDirection * moveSpeed, 0);
            float leftBound = initialPosition.x - patrolRange;
            float rightBound = initialPosition.x + patrolRange;
            if (transform.position.x >= rightBound && patrolDirection > 0)
            {
                patrolDirection = -1;
            }
            else if (transform.position.x <= leftBound && patrolDirection < 0)
            {
                patrolDirection = 1;
            }
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.flipX = (patrolDirection < 0);
            }
            // Keep the enemy at its initial flying altitude.
            transform.position = new Vector3(transform.position.x, initialPosition.y, transform.position.z);
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
}
