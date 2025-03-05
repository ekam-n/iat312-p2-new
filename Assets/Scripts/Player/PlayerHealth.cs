using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;    // Maximum health for the player
    private float currentHealth;
    private Animator anim;            // Optional: to play damage/death animations

    void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
    }

    // Call this method to apply damage to the player.
    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log("Player takes " + damageAmount + " damage. Current health: " + currentHealth);

        // Optionally, play a damage animation.
        if (anim != null)
        {
            anim.SetTrigger("Damage");
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Handle the player's death.
    private void Die()
    {
        Debug.Log("Player died!");
        // Optionally, play a death animation, disable controls, or reload the scene.
        // For now, we just destroy the player.
        Destroy(gameObject);
    }
}
