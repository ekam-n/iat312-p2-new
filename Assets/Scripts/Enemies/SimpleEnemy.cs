using UnityEngine;

public class SimpleEnemy : EnemyBase
{
    [Header("Patrol Settings")]
    public float patrolRange = 5f;
    protected Vector3 initialPosition;
    protected int patrolDirection = 1;

    [Header("Vision Settings")]
    public float visionRange = 10f;
    public float visionAngle = 45f;
    protected bool isChasing = false;
    public float lostSightTimeThreshold = 3f;
    protected float lostSightTimer = 0f;

    [Header("Attack Settings")]
    public float attackCooldown = 2f;
    protected float attackTimer = 0f;
    protected PlayerHealth collidedPlayerHealth;

    [Header("Target")]
    public Transform target;

    [Header("Obstacle Settings")]
    public LayerMask obstacleMask;

    [Header("Chase Settings")]
    public float chaseSpeedMultiplier = 1.5f;

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
        if (Input.GetKeyDown(KeyCode.V))
        {
            showVisionCone = !showVisionCone;
        }

        if (target != null)
        {
            Vector2 toPlayer = target.position - transform.position;
            if (toPlayer.magnitude <= visionRange)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, toPlayer.normalized, toPlayer.magnitude, obstacleMask);
                if (hit.collider == null)
                {
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
            attackTimer -= Time.deltaTime;
        }
        else
        {
            Patrol();
            attackTimer = attackCooldown;
        }
    }

    public override void Patrol()
    {
        if (target != null)
        {
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
        
        }
    }

}