using UnityEngine;
using System.Collections;

public class SlowEffect : MonoBehaviour
{
    [Tooltip("Multiplier to apply to the enemy's speed (e.g., 0.5 means 50% of original).")]
    public float slowMultiplier = 0.5f;
    [Tooltip("Duration of the slow effect in seconds.")]
    public float slowDuration = 3f;

    private EnemyBase enemy;
    private float originalSpeed;
    private Coroutine slowCoroutine;

    void Awake()
    {
        enemy = GetComponent<EnemyBase>();

        if (enemy != null)
        {
            // Save the enemy's original speed and apply the slow.
            originalSpeed = enemy.moveSpeed;
            enemy.moveSpeed *= slowMultiplier;
            slowCoroutine = StartCoroutine(SlowRoutine());
        }
        else
        {
            Debug.LogError("EnemyBase component not found on " + gameObject.name);
        }
    }

    // Call this to reset the slow duration if the enemy is hit again.
   public void ResetDuration()
{
    // Only start the coroutine if the GameObject is active
    if (gameObject.activeInHierarchy)
    {
        if (slowCoroutine != null)
        {
            StopCoroutine(slowCoroutine);
        }
        slowCoroutine = StartCoroutine(SlowRoutine());
    }
    else
    {
        Debug.LogWarning("Cannot start coroutine on an inactive GameObject.");
    }
}


    IEnumerator SlowRoutine()
    {
        yield return new WaitForSeconds(slowDuration);
        if (enemy != null)
        {
            // Restore the original speed.
            enemy.moveSpeed = originalSpeed;
        }
        Destroy(this);
    }
}
