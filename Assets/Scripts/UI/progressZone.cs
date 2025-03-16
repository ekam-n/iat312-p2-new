using UnityEngine;

public class progressZone : MonoBehaviour
{
    public float levelIncrease;

    void OnTriggerEnter2D(Collider2D other)
    {
        levelProgress lp = FindAnyObjectByType<levelProgress>(); // Get the levelProgress component in the scene
        if (lp != null)
        {
            lp.progressTracker(levelIncrease); // Call the method directly
        }
        else
        {
            Debug.LogWarning("No levelProgress script found in the scene!");
        }
    }
}
