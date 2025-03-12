using UnityEngine;

public class EnemyDoor : MonoBehaviour
{
    [Header("Required Enemies")]
    [Tooltip("List of enemy GameObjects that must be defeated before the door opens.")]
    public GameObject[] requiredEnemies;

    // A flag to ensure the door is only opened once.
    private bool doorOpened = false;

    void Update()
    {
        if (doorOpened)
            return;

        bool allDefeated = true;

        // Check if every enemy in the list is null (destroyed).
        foreach (GameObject enemy in requiredEnemies)
        {
            if (enemy != null)
            {
                allDefeated = false;
                break;
            }
        }

        // If all enemies are defeated, open (deactivate) the door.
        if (allDefeated)
        {
            gameObject.SetActive(false);
            doorOpened = true;
            Debug.Log("All required enemies defeated. Door is now open.");
        }
    }
}
