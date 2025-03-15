using UnityEngine;
using UnityEngine.UI;
using System;  // Add this for Action

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;    // Maximum health for the player
    public float currentHealth;
    public Image healthBar;
    private Animator anim;            // Optional: to play damage/death animations
    
    // Add this event for the checkpoint system
    public event Action OnPlayerDied;

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
        
        // Invoke the event instead of destroying the player
        OnPlayerDied?.Invoke();
        
        // Disable the player temporarily instead of destroying
        gameObject.SetActive(false);
    }
    
    // Add this method for the checkpoint system
    public void ResetHealth()
    {
        currentHealth = maxHealth;
        healthBar.fillAmount = Mathf.Clamp(currentHealth / maxHealth, 0, 1);
    }
}