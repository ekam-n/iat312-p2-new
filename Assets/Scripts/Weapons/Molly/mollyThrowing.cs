using UnityEngine;

public class mollyThrowing : MonoBehaviour
{
    public GameObject molotovPrefab;
    public Transform throwPoint;
    public float throwForce = 10f;
    [Tooltip("Angular velocity (in degrees per second) to spin the molotov when thrown.")]
    public float spinSpeed = 360f; // Adjust as needed

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift)) // Change input as needed
        {
            ThrowMolotov();
        }
    }

    void ThrowMolotov()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0; // Ensure it's in 2D space

        // Calculate the direction from the throwPoint to the mouse.
        Vector2 throwDirection = (mousePos - throwPoint.position).normalized;
        GameObject molotov = Instantiate(molotovPrefab, throwPoint.position, Quaternion.identity);
        Rigidbody2D rb = molotov.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // Set the linear velocity for the throw.
            rb.linearVelocity = throwDirection * throwForce;
            // Set the angular velocity so the molotov rotates.
            rb.angularVelocity = spinSpeed;
        }
    }
}
