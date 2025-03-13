using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;    // Maximum health for the player
    public float currentHealth;
    public Image healthBar;
    private Animator anim;            // Optional: to play damage/death animations

    void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        healthBar.fillAmount = Mathf.Clamp(currentHealth / maxHealth, 0, 1);
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
            healthBar.fillAmount = Mathf.Clamp(0 / maxHealth, 0, 1);
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
