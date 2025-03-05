using UnityEngine;

public class SimpleEnemy : EnemyBase
{
    [Header("Simple Enemy Settings")]
    public Transform target;        // Typically the player's transform.
    public float attackRange = 1f;
    public float attackCooldown = 2f;
    private float attackTimer;

    protected override void Awake()
    {
        base.Awake();
        // Optionally, if target isn't manually assigned, try to find the player by tag.
        if(target == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                target = playerObj.transform;
        }
    }

    void Update()
    {
        Patrol();

        attackTimer -= Time.deltaTime;
        if(target != null && Vector2.Distance(transform.position, target.position) <= attackRange && attackTimer <= 0f)
        {
            PerformAttack();
            attackTimer = attackCooldown;
        }
    }

    // A very basic patrol: simply move toward the target.
    public override void Patrol()
    {
        if(target != null)
        {
            Vector2 direction = (target.position - transform.position).normalized;
            rb.linearVelocity = direction * moveSpeed;
            
            // Optionally flip sprite based on movement direction.
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            if(sr != null)
            {
                sr.flipX = (direction.x < 0);
            }
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    // Implement a basic attack.
    public override void PerformAttack()
    {
        // This is where you might call a method on the player to reduce their health.
        Debug.Log("SimpleEnemy attacks for " + damage + " damage.");
    }
}
