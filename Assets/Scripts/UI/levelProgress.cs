using UnityEngine;
using UnityEngine.UI;

public class levelProgress : MonoBehaviour
{
    public float maxProgress = 100f;    
    public float currentProgress;
    public Image progressBar;

    // Update is called once per frame
    private void Update()
    {
        progressBar.fillAmount = Mathf.Clamp(currentProgress / maxProgress, 0, 1);
    }
    public void progressTracker(float a)
    {
        currentProgress = a;
    }
}
