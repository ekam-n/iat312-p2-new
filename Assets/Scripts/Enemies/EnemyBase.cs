using UnityEngine;
using System.Collections;

public abstract class EnemyBase : MonoBehaviour
{
    [Header("Enemy Stats")]
    public float health = 100f;
    public float moveSpeed = 10f;
    public int damage = 10;

    protected Animator anim;
    protected Rigidbody2D rb;
    
    // New flag to track tranquilization status.
    protected bool isTranquilized = false;

    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Apply damage to the enemy.
    public virtual void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }

    // Common death behavior.
    protected virtual void Die()
    {
        Destroy(gameObject);
    }

    // Abstract methods for enemy behavior.
    public abstract void PerformAttack();
    public abstract void Patrol();

    // Tranquilize the enemy (puts it to sleep).
    public virtual void Tranquilize(float duration)
    {
        StartCoroutine(TranquilizeRoutine(duration));
    }

    private IEnumerator TranquilizeRoutine(float duration)
    {
        isTranquilized = true;
        if (anim != null)
        {
            anim.SetBool("isSleeping", true);
        }

        // Disable movement while tranquilized.
        float originalSpeed = moveSpeed;
        moveSpeed = 0;

        yield return new WaitForSeconds(duration);

        // Restore movement and mark enemy as awake.
        moveSpeed = originalSpeed;
        if (anim != null)
        {
            anim.SetBool("isSleeping", false);
        }
        isTranquilized = false;
    }
}
