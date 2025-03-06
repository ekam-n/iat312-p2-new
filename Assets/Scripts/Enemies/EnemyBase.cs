using UnityEngine;
using System.Collections; 

public abstract class EnemyBase : MonoBehaviour
{
    [Header("Enemy Stats")]
    public float health = 100f;
    public float moveSpeed = 2f;
    public int damage = 10;

    protected Animator anim;
    protected Rigidbody2D rb;

    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Call this method to apply damage to the enemy.
    public virtual void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }

    // Common death behavior (can be overridden).
    protected virtual void Die()
    {
        // Optionally play a death animation, disable colliders, etc.
        // For now, simply destroy the enemy.
        Destroy(gameObject);
    }

    // Abstract methods that each enemy type must implement.
    public abstract void PerformAttack();
    public abstract void Patrol();

    // New method to put the enemy to sleep (tranquilize).
    public virtual void Tranquilize(float duration)
    {
        StartCoroutine(TranquilizeRoutine(duration));
    }

    private IEnumerator TranquilizeRoutine(float duration)
    {
        // Optionally, play a sleep animation.
        if (anim != null)
        {
            anim.SetBool("isSleeping", true);
        }

        // Store the original move speed.
        float originalSpeed = moveSpeed;
        moveSpeed = 0;  // Disable movement

        // Wait for the duration.
        yield return new WaitForSeconds(duration);

        // Wake the enemy up.
        moveSpeed = originalSpeed;
        if (anim != null)
        {
            anim.SetBool("isSleeping", false);
        }
    }
}
