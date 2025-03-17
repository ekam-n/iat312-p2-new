using UnityEngine;

public class Pineapple : MonoBehaviour
{
    [Tooltip("The amount of health to restore when the player picks up this item.")]
    public float healthAmount = 25f;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if(playerHealth != null)
            {
                // Increase player's health without exceeding 100f.
                playerHealth.health = Mathf.Min(playerHealth.health + healthAmount, 100f);
                Debug.Log("Pineapple picked up! Player's health is now " + playerHealth.health);
                Destroy(gameObject);
            }
        }
    }
}
