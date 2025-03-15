using UnityEngine;

public class ZoneTriggerCheckpoint : MonoBehaviour
{
    [Header("Checkpoint Settings")]
    public bool isCheckpoint = true;
    public CheckpointSystem checkpointSystem;
    public Vector3 respawnOffset = new Vector3(0, 0.5f, 0);
    
    [Header("Visual Feedback")]
    public GameObject checkpointActiveEffect;
    public GameObject checkpointInactiveEffect;
    
    private bool hasBeenActivated = false;
    
    private void Start()
    {
        // Ensure visual state is correct at start
        if (checkpointActiveEffect != null)
            checkpointActiveEffect.SetActive(false);
            
        if (checkpointInactiveEffect != null)
            checkpointInactiveEffect.SetActive(true);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if this is a checkpoint and the colliding object is the player
        if (!isCheckpoint || !other.CompareTag("Player"))
            return;
            
        // Only activate once
        if (hasBeenActivated)
            return;
            
        Debug.Log("Player entered checkpoint: " + gameObject.name);
        
        // Safely set checkpoint
        try {
            // Calculate respawn position
            Vector3 checkpointPosition = transform.position + respawnOffset;
            
            // Set this as the new checkpoint if the system exists
            if (checkpointSystem != null)
            {
                checkpointSystem.SetCheckpoint(checkpointPosition);
                ActivateCheckpointVisuals();
                hasBeenActivated = true;
                Debug.Log("Checkpoint activated successfully!");
            }
            else
            {
                Debug.LogWarning("No CheckpointSystem assigned to " + gameObject.name);
            }
        }
        catch (System.Exception e) {
            Debug.LogError("Error in checkpoint trigger: " + e.Message);
        }
    }
    
    private void ActivateCheckpointVisuals()
    {
        try {
            // Deactivate inactive visual
            if (checkpointInactiveEffect != null)
                checkpointInactiveEffect.SetActive(false);
            
            // Activate active visual
            if (checkpointActiveEffect != null)
                checkpointActiveEffect.SetActive(true);
        }
        catch (System.Exception e) {
            Debug.LogError("Error changing checkpoint visuals: " + e.Message);
        }
    }
}