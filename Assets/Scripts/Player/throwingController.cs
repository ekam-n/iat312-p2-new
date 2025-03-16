using UnityEngine;

public class throwingController : MonoBehaviour
{
    public GameObject molotovPrefab;
    public GameObject coconutPrefab;
    public Transform throwPoint;
    public float throwForce = 10f;
    [Tooltip("Angular velocity (in degrees per second) to spin the molotov when thrown.")]
    public float spinSpeed = 360f; // Adjust as needed

    public int mollyAmmo = 0;
    public int maxMolly = 5;

    public int cocoAmmo = 0;
    public int maxCoco = 5;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && mollyAmmo !=0) // Change input as needed
        {
            ThrowMolotov();
            mollyAmmo -= 1;
        }

        if (Input.GetKeyDown(KeyCode.E) && cocoAmmo !=0) // Change input as needed
        {
            ThrowCoconut();
            cocoAmmo -= 1;
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
    void ThrowCoconut()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0; // Ensure it's in 2D space

        // Calculate the direction from the throwPoint to the mouse.
        Vector2 throwDirection = (mousePos - throwPoint.position).normalized;
        GameObject coconut = Instantiate(coconutPrefab, throwPoint.position, Quaternion.identity);
        Rigidbody2D rb = coconut.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // Set the linear velocity for the throw.
            rb.linearVelocity = throwDirection * throwForce;
            // Set the angular velocity so the molotov rotates.
            rb.angularVelocity = spinSpeed;
        }
    }
   
}
