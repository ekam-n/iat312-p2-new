using UnityEngine;

public class throwPointController : MonoBehaviour
{
    public Transform player; // Assign the player's transform in the Inspector
    public float distanceFromPlayer = 1f; // Adjust this in the Inspector for best results

    void Update()
    {
        // Get the mouse position in world space
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0; // Ensure it's in 2D space

        // Calculate direction from the player to the mouse
        Vector2 direction = (mousePos - player.position).normalized;

        // Set throwPoint position around the player
        transform.position = (Vector2)player.position + direction * distanceFromPlayer;
    }
}
