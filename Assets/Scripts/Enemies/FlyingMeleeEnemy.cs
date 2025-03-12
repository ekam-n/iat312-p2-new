using UnityEngine;
using System.Collections;

public class FlyingMeleeEnemy : SimpleEnemy
{
    // We override Awake to disable gravity.
    protected override void Awake()
    {
        base.Awake();
        // Disable gravity so this enemy can fly.
        rb.gravityScale = 0f;
        // Store the flying altitude (y) from its starting position.
        initialPosition = transform.position; 
    }

    // Override Update to change the chase behavior.
    // Note: Since SimpleEnemy's Update isn’t virtual, we use "new" to hide it.
    new void Update()
    {
        // Toggle vision cone display when V is pressed.
        if (Input.GetKeyDown(KeyCode.V))
        {
            showVisionCone = !showVisionCone;
        }

        // Vision check (same as in SimpleEnemy).
        if (target != null)
        {
            Vector2 toPlayer = target.position - transform.position;
            if (toPlayer.magnitude <= visionRange)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, toPlayer.normalized, toPlayer.magnitude, obstacleMask);
                if (hit.collider == null)
                {
                    // Use patrolDirection as the forward when not chasing.
                    Vector2 forward = new Vector2(patrolDirection, 0);
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
                rb.linearVelocity = direction * moveSpeed;
                // Flip sprite based on horizontal movement.
                SpriteRenderer sr = GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    sr.flipX = (direction.x < 0);
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
            // In patrol mode, the enemy flies left/right at a fixed altitude.
            Patrol();
            attackTimer = attackCooldown;
        }
    }

    // Override Patrol so that vertical position remains constant.
    public override void Patrol()
    {
        if (target != null)
        {
            // Move only in x, but maintain the initial y (flying altitude).
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
            // Ensure the enemy stays at the flying altitude.
            transform.position = new Vector3(transform.position.x, initialPosition.y, transform.position.z);
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
}
