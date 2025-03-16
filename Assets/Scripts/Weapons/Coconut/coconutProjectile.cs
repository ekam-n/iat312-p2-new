using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class coconutProjectile : MonoBehaviour
{
    public float attractionRadius = 5f; // Radius within which enemies will be attracted
    public float attractionDuration = 5f; // Time enemies stay focused on the coconut
    public LayerMask groundLayer; // Layer that represents the ground

    private bool hasLanded = false; // Flag to ensure attraction happens only once

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the coconut hits the ground
        if (!hasLanded && ((1 << collision.gameObject.layer) & groundLayer) != 0)
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
            col.isTrigger = true; // Make it a trigger so enemies don't push it
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
                enemy.target = transform; // Set the coconut as their target
                enemy.isChasing = true;

                // Ensure AI doesn't reset its patrol by disabling its vision checks temporarily
                StartCoroutine(KeepEnemyFocused(enemy));
            }
        }
    }

    IEnumerator KeepEnemyFocused(SimpleEnemy enemy)
    {
        float elapsedTime = 0f;
        float stopDistance = 5.5f;
        while (elapsedTime < attractionDuration)
        {
            if (enemy != null && enemy.target != null)
            {
                Vector2 coconutPosition = (Vector2)transform.position;
                float distanceToCoconut = Vector2.Distance(enemy.transform.position, enemy.target.position);

                // Only move if not within the stop distance
                if (distanceToCoconut > stopDistance)
                {
                    enemy.isChasing = true; // Keep enemy chasing
                }
                else
                {
                    enemy.isChasing = false;
                    enemy.rb.linearVelocity = Vector2.zero;

                    // **Offset Logic to Prevent Overlapping**
                    Vector2 offsetDirection = (enemy.transform.position - transform.position).normalized;
                    if (offsetDirection == Vector2.zero)
                    {
                        offsetDirection = Vector2.right; // Default direction in case enemy is exactly on top
                    }
                    enemy.transform.position = coconutPosition + offsetDirection * stopDistance;
                }
            }
            elapsedTime += Time.deltaTime;
            yield return null; // Wait one frame and repeat
        }
    }

    IEnumerator HandleCoconutLifecycle()
    {
        yield return new WaitForSeconds(attractionDuration);
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        // Reset enemies' targets
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, attractionRadius);
        foreach (Collider2D enemyCollider in enemies)
        {
            SimpleEnemy enemy = enemyCollider.GetComponent<SimpleEnemy>();
            if (enemy != null)
            {
                if (playerObj != null)
                {
                    enemy.target = playerObj.transform; // Restore target to player
                }
                enemy.isChasing = false; // Stop chasing

                // Reset movement values to ensure patrol resumes
                enemy.lostSightTimer = 0f;
                enemy.attackTimer = enemy.attackCooldown; // Reset attack timer
                enemy.rb.linearVelocity = Vector2.zero; // Stop movement briefly

                // Manually call patrol method to ensure they start moving again
                StartCoroutine(ForceEnemyPatrol(enemy));

            }
        }

        // Destroy the coconut after the effect duration
        Destroy(gameObject);
    }
    IEnumerator ForceEnemyPatrol(SimpleEnemy enemy)
    {
        yield return new WaitForSeconds(0.1f); // Small delay to avoid AI conflict

        if (enemy != null && !enemy.isChasing)
        {
            // Ensure the patrol direction is valid
            if (enemy.patrolDirection == 0)
            {
                enemy.patrolDirection = Random.value > 0.5f ? 1 : -1; // Pick a random patrol direction
            }
            //enemy.lostSightTimer = 0f;
            enemy.rb.linearVelocity = Vector2.zero;
            enemy.isChasing = false;
            yield return null;
            enemy.rb.linearVelocity = new Vector2(enemy.patrolDirection * enemy.moveSpeed, enemy.rb.linearVelocity.y);
            enemy.Patrol();
        }
    }

}
