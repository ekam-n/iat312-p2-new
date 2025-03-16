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
            }
        }
    }

    IEnumerator HandleCoconutLifecycle()
    {
        yield return new WaitForSeconds(attractionDuration);

        // Reset enemies' targets
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, attractionRadius);
        foreach (Collider2D enemyCollider in enemies)
        {
            SimpleEnemy enemy = enemyCollider.GetComponent<SimpleEnemy>();
            if (enemy != null)
            {
                enemy.target = null; // Reset target
                enemy.isChasing = false; // Stop chasing

                // Reset movement values to ensure patrol resumes
                enemy.lostSightTimer = 0f;
                enemy.attackTimer = enemy.attackCooldown; // Reset attack timer
                enemy.rb.linearVelocity = Vector2.zero; // Stop movement briefly

                // Manually call patrol method to ensure they start moving again
                enemy.Patrol();
            }
        }

        // Destroy the coconut after the effect duration
        Destroy(gameObject);
    }
}
