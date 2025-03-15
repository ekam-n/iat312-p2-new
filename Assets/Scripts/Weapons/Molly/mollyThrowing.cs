using UnityEngine;

public class mollyThrowing : MonoBehaviour
{
    public GameObject molotovPrefab;
    public Transform throwPoint;
    public float throwForce = 10f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift)) // Change this to your preferred input
        {
            ThrowMolotov();
        }
    }

    void ThrowMolotov()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0; // Ensure it's in 2D space

        // Calculate the direction from the player to the mouse
        Vector2 throwDirection = (mousePos - throwPoint.position).normalized;
        GameObject molotov = Instantiate(molotovPrefab, throwPoint.position, Quaternion.identity);
        Rigidbody2D rb = molotov.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = throwDirection * throwForce; // Adjust for an arcing throw
        }
    }
}