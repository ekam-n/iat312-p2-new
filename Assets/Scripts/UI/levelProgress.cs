using UnityEngine;
using UnityEngine.UI;

public class levelProgress : MonoBehaviour
{
    // We'll work with normalized progress (0 to 1), so set maxProgress to 1.
    public float maxProgress = 1f;    
    public float currentProgress = 0f;
    public Image progressBar;

    void Update()
    {
        // Update the progress bar fill amount with a normalized value.
        progressBar.fillAmount = Mathf.Clamp(currentProgress / maxProgress, 0, 1);
    }

    // Instead of setting currentProgress, this method adds to it.
    public void progressTracker(float a)
    {
        currentProgress += a;
        currentProgress = Mathf.Clamp(currentProgress, 0, maxProgress);
    }

    // A property to access the normalized progress.
    public float progress
    {
        get { return currentProgress / maxProgress; }
        set { currentProgress = Mathf.Clamp(value, 0, 1) * maxProgress; }
    }
}
