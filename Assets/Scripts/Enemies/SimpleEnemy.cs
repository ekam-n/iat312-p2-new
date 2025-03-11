using UnityEngine;

public class SimpleEnemy : EnemyBase
{
    [Header("Simple Enemy Settings")]
    public Transform target;         // Typically the player's transform.
    public float attackCooldown = 2f;  // Time between damage ticks when colliding.
    private float attackTimer;
    private bool isCollidingWithPlayer = false;
    private PlayerHealth collidedPlayerHealth;

    protected override void Awake()
    {
        base.Awake();
        // If target isn't manually assigned, try to find the player by tag.
        if (target == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                target = playerObj.transform;
        }
        attackTimer = attackCooldown;
    }

    void Update()
    {
        if (isCollidingWithPlayer)
        {
            rb.linearVelocity = Vector2.zero; // Stop moving while colliding
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0f)
            {
                PerformAttack();
                attackTimer = attackCooldown;
            }
        }
        else
        {
            // Patrol toward the target when not colliding.
            Patrol();
            attackTimer = attackCooldown;
        }
    }

    // Patrol: move toward the target.
    public override void Patrol()
    {
        if (target != null)
        {
            Vector2 direction = (target.position - transform.position).normalized;
            rb.linearVelocity = direction * moveSpeed;
            
            // Optionally, flip the enemy's sprite based on movement direction.
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
    }

    // PerformAttack: deal damage to the player.
    public override void PerformAttack()
    {
        if (collidedPlayerHealth != null)
        {
            collidedPlayerHealth.TakeDamage(damage);
            Debug.Log("SimpleEnemy attacks for " + damage + " damage.");
        }
    }

    // When collision with the player begins.
    void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerHealth ph = collision.gameObject.GetComponent<PlayerHealth>();
        if (ph != null)
        {
            isCollidingWithPlayer = true;
            collidedPlayerHealth = ph;
            
            // Immediate first attack
            PerformAttack();
            
            // Reset timer for cooldown-based follow-up attacks
            attackTimer = attackCooldown;
        }
    }


    // When collision with the player ends.
    void OnCollisionExit2D(Collision2D collision)
    {
        PlayerHealth ph = collision.gameObject.GetComponent<PlayerHealth>();
        if (ph != null)
        {
            isCollidingWithPlayer = false;
            collidedPlayerHealth = null;
            attackTimer = attackCooldown;
        }
    }
}
