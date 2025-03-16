using UnityEngine;
using System.Collections;

public class SimpleEnemy : EnemyBase
{
    [Header("Patrol Settings")]
    public float patrolRange = 5f;    // Half the patrol width.
    protected Vector3 initialPosition;
    protected int patrolDirection = 1;  // 1 = moving right, -1 = moving left.

    [Header("Vision Settings")]
    public float visionRange = 10f;   // Maximum distance at which the enemy can see the player.
    public float visionAngle = 45f;   // Half-angle of the vision cone (in degrees).
    public bool isChasing = false;   // Whether the enemy has detected the player.
    public float lostSightTimeThreshold = 3f; // Time enemy will chase without seeing the player before giving up.
    public float lostSightTimer = 0f;         // Timer for how long the enemy hasn't seen the player.

    [Header("Attack Settings")]
    public float attackCooldown = 2f; // Time between consecutive attacks when colliding.
    public float attackTimer = 0f;
    protected PlayerHealth collidedPlayerHealth;

    [Header("Target")]
    public Transform target;          // Typically the player's transform.

    [Header("Obstacle Settings")]
    public LayerMask obstacleMask;    // Layers that block the enemy's vision (e.g., walls, ground).

    [Header("Chase Settings")]
    [Tooltip("Multiplier applied to moveSpeed when chasing the player.")]
    public float chaseSpeedMultiplier = 1.5f;

    // Debug: toggle drawing the vision cone.
    protected bool showVisionCone = false;

    protected override void Awake()
    {
        base.Awake();
        initialPosition = transform.position;
        if (target == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                target = playerObj.transform;
        }
        attackTimer = attackCooldown;
        lostSightTimer = 0f;
    }

    void Update()
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
                // Cast a ray toward the player.
                RaycastHit2D hit = Physics2D.Raycast(transform.position, toPlayer.normalized, toPlayer.magnitude, obstacleMask);
                if (hit.collider == null)
                {
                    // No obstacle blocking view; now check vision cone.
                    Vector2 forward = new Vector2(patrolDirection, 0); // enemy's "forward" in patrol mode.
                    float angleToPlayer = Vector2.Angle(forward, toPlayer);
                    if (angleToPlayer <= visionAngle)
                    {
                        // Player is seen. Reset lostSightTimer.
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
                    // Obstacle detected. If already chasing, increment lost sight timer.
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
            // In chase mode, move horizontally toward the player's x position
            // while preserving vertical velocity so gravity affects the enemy.
            if (target != null)
            {
                float directionX = Mathf.Sign(target.position.x - transform.position.x);
                rb.linearVelocity = new Vector2(directionX * moveSpeed * chaseSpeedMultiplier, rb.linearVelocity.y);
                
                // Flip sprite based on horizontal direction.
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
            // In patrol mode, move left/right within patrol range.
            Patrol();
            attackTimer = attackCooldown;
        }
    }

    public override void Patrol()
    {
        if (target != null)
        {
            // Patrol only in x direction.
            rb.linearVelocity = new Vector2(patrolDirection * moveSpeed, rb.linearVelocity.y);
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
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    public override void PerformAttack()
    {
        if (isTranquilized)
        {
            Debug.Log("Enemy is tranquilized and cannot attack.");
            return;
        }
        if (collidedPlayerHealth != null)
        {
            collidedPlayerHealth.TakeDamage(damage);
            Debug.Log("SimpleEnemy attacks for " + damage + " damage.");
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerHealth ph = collision.gameObject.GetComponent<PlayerHealth>();
        if (ph != null)
        {
            collidedPlayerHealth = ph;
            if (!isTranquilized)
            {
                PerformAttack();
                attackTimer = attackCooldown;
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        PlayerHealth ph = collision.gameObject.GetComponent<PlayerHealth>();
        if (ph != null)
        {
            collidedPlayerHealth = null;
            attackTimer = attackCooldown;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collidedPlayerHealth != null && attackTimer <= 0f)
        {
            PerformAttack();
            attackTimer = attackCooldown;
        }
    }

    // Draw the vision cone for debugging when toggled.
    void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            return;
        if (showVisionCone)
        {
            Gizmos.color = Color.yellow;
            Vector3 startPos = transform.position;
            Vector2 forward;
            if (!isChasing)
            {
                forward = new Vector2(patrolDirection, 0);
            }
            else if (target != null)
            {
                // When chasing, only consider horizontal direction.
                forward = new Vector2(Mathf.Sign(target.position.x - transform.position.x), 0);
            }
            else
            {
                forward = Vector2.right;
            }
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
