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

    // **Shooting with Left Mouse Click**
    if (Input.GetMouseButtonDown(0) && Time.time >= nextFireTime)
    {
        nextFireTime = Time.time + fireRate;
        Shoot();
    }
}

    void Shoot()
{
    if (projectilePrefab == null)
    {
        Debug.LogError("Projectile Prefab is not assigned in the Inspector.");
        return;
    }

    // Get the mouse position in world space
    Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    mousePosition.z = 0f; // Ensure no depth issues in 2D

    // Calculate direction from firePoint to mouse
    Vector2 shootDirection = (mousePosition - firePoint.position).normalized;

    // Calculate the rotation angle (ensure the projectile is properly aligned)
    float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;

    // Instantiate the projectile and apply rotation
    GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.Euler(0, 0, angle - 90)); // Adjust for capsule orientation

    // Pass the direction to the projectile
    Projectile projectileScript = projectile.GetComponent<Projectile>();
    if (projectileScript != null)
    {
        projectileScript.SetDirection(shootDirection);
    }
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
