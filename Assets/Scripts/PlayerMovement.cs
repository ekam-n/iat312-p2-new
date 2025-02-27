using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public GameObject projectilePrefab;  // Assign projectile prefab in Unity
    public Transform firePoint; // Empty GameObject to set projectile spawn point
    public float fireRate = 0.5f; // Time between shots
    private float nextFireTime = 0f;
    
    private Rigidbody2D rb;
    private bool isGrounded;
    private bool facingRight = true; // Track player direction
    private bool isFirstFrame = true; // Track if it's the first frame

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Prevent firing on the first frame
        if (isFirstFrame)
        {
            isFirstFrame = false;
            return; // Exit early if it's the first frame of the game
        }

        // Movement (Left/Right) - Supports Arrow Keys and A/D
        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // Flip player sprite if moving left/right
        if (moveInput > 0) {
            facingRight = true;
            transform.localScale = new Vector3(1, 1, 1); // Reset scale
        }
        else if (moveInput < 0) {
            facingRight = false;
            transform.localScale = new Vector3(-1, 1, 1); // Flip horizontally
        }

        // Jumping
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        // Shooting - Ensure continuous firing with correct fire rate
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        // Check if the projectilePrefab is assigned
        if (projectilePrefab == null)
        {
            Debug.LogError("Projectile Prefab is not assigned in the Inspector.");
            return;
        }

        // Instantiate the projectile at the fire point
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        // Set projectile direction based on player's facing direction
        Projectile projectileScript = projectile.GetComponent<Projectile>();
        if (projectileScript != null)
        {
            projectileScript.SetDirection(facingRight ? 1 : -1); // 1 for right, -1 for left
        }

        // Rotate the projectile by 90 degrees (if required)
        projectile.transform.Rotate(0, 0, 90); // Rotate 90 degrees on the Z-axis
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
