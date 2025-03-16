using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class coconutProjectile : MonoBehaviour
{
    [Header("Attraction Settings")]
    public float attractionRadius = 5f; // Radius within which enemies will be attracted
    public float attractionDuration = 5f; // Time enemies stay focused on the coconut
    [Tooltip("Distance from the coconut at which attracted enemies stop moving toward it.")]
    public float stopDistance = 2f; // Desired offset distance

    [Header("Ground Settings")]
    public LayerMask groundLayer; // Layer representing the ground

    private bool hasLanded = false; // Ensure attraction happens only once

    void OnCollisionEnter2D(Collision2D collision)
    {
        // If the coconut hits the ground (or an enemy), and hasn't already landed…
        if (!hasLanded && (((1 << collision.gameObject.layer) & groundLayer) != 0))
        {
            hasLanded = true;
            StopPhysics();
            AttractNearbyEnemies();
            StartCoroutine(HandleCoconutLifecycle());
        }
    }

    void StopPhysics()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            col.isTrigger = true; // so enemies don't push it
        }
    }

    void AttractNearbyEnemies()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, attractionRadius);
        foreach (Collider2D enemyCollider in enemies)
        {
            SimpleEnemy enemy = enemyCollider.GetComponent<SimpleEnemy>();
            if (enemy != null)
            {
                // Calculate the direction from the enemy to the coconut.
                Vector2 direction = ((Vector2)transform.position - (Vector2)enemy.transform.position).normalized;
                float distance = Vector2.Distance(transform.position, enemy.transform.position);
                // Cast a ray from the enemy to the coconut; if an obstacle is in between, skip this enemy.
                RaycastHit2D groundHit = Physics2D.Raycast(enemy.transform.position, direction, distance, groundLayer);
                if (groundHit.collider != null)
                {
                    continue;
                }
                
                // Attract the enemy:
                enemy.target = transform; // set the coconut as its target
                enemy.isChasing = true;   // flag it as attracted

                // Continuously force the enemy to remain at the proper offset.
                StartCoroutine(KeepEnemyFocused(enemy));
            }
        }
    }

    IEnumerator KeepEnemyFocused(SimpleEnemy enemy)
    {
        float elapsedTime = 0f;
        while (elapsedTime < attractionDuration)
        {
            if (enemy != null && enemy.target != null)
            {
                Vector2 coconutPos = (Vector2)transform.position;
                float currentDistance = Vector2.Distance(enemy.transform.position, coconutPos);

                if (currentDistance > stopDistance)
                {
                    // Let the enemy move normally toward the coconut.
                    enemy.isChasing = true;
                }
                else
                {
                    // Once the enemy is close enough, continuously pull it to the desired offset.
                    enemy.isChasing = false;
                    enemy.rb.linearVelocity = Vector2.zero;

                    // Calculate desired position: enemy should be exactly stopDistance away horizontally
                    // and its vertical coordinate should match the coconut's.
                    Vector2 horizontalDir = enemy.transform.position - transform.position;
                    horizontalDir.y = 0; // only horizontal
                    if (horizontalDir == Vector2.zero)
                    {
                        horizontalDir = Vector2.right; // default if exactly aligned
                    }
                    horizontalDir.Normalize();

                    Vector2 desiredPos = coconutPos + horizontalDir * stopDistance;

                    // Smoothly move the enemy toward desiredPos.
                    // Using MovePosition ensures smooth physics-based movement.
                    float smoothSpeed = 10f; // adjust this value to control "magnet" speed
                    Vector2 newPos = Vector2.Lerp(enemy.transform.position, new Vector2(desiredPos.x, coconutPos.y), smoothSpeed * Time.fixedDeltaTime);
                    enemy.rb.MovePosition(newPos);
                }
            }
            elapsedTime += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
    }


    IEnumerator HandleCoconutLifecycle()
    {
        yield return new WaitForSeconds(attractionDuration);
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        // Reset enemies' targets.
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, attractionRadius);
        foreach (Collider2D enemyCollider in enemies)
        {
            SimpleEnemy enemy = enemyCollider.GetComponent<SimpleEnemy>();
            if (enemy != null)
            {
                if (playerObj != null)
                {
                    enemy.target = playerObj.transform; // restore target to player
                }
                enemy.isChasing = false;

                // Reset movement so patrol resumes.
                enemy.lostSightTimer = 0f;
                enemy.attackTimer = enemy.attackCooldown;
                enemy.rb.linearVelocity = Vector2.zero;
                StartCoroutine(ForceEnemyPatrol(enemy));
            }
        }

        // Destroy the coconut after the effect.
        Destroy(gameObject);
    }

    IEnumerator ForceEnemyPatrol(SimpleEnemy enemy)
    {
        yield return new WaitForSeconds(0.1f);
        if (enemy != null && !enemy.isChasing)
        {
            if (enemy.patrolDirection == 0)
            {
                enemy.patrolDirection = Random.value > 0.5f ? 1 : -1;
            }
            enemy.rb.linearVelocity = new Vector2(enemy.patrolDirection * enemy.moveSpeed, enemy.rb.linearVelocity.y);
            enemy.Patrol();
        }
    }
}
