using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireRate = 0.5f;
    private float nextFireTime = 0f;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer spriteRenderer; // ✅ Reference to SpriteRenderer
    private bool isGrounded;
    private bool facingRight = true;
    private bool isFirstFrame = true;
    
    private bool hasWeapon = false; // Player starts without a weapon

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // ✅ Get SpriteRenderer Component
    }

    void Update()
{
    if (isFirstFrame)
    {
        isFirstFrame = false;
        return;
    }

    float moveInput = Input.GetAxisRaw("Horizontal");
    rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

    // ✅ Ensure isMoving updates every frame
    bool isMoving = Mathf.Abs(moveInput) > 0.01f;
    anim.SetBool("isMoving", isMoving);

    // ✅ Prevent running animation from overriding the jump
    if (isGrounded)
    {
        anim.SetBool("isMoving", isMoving);
    }

    if (moveInput > 0)
    {
        facingRight = true;
        transform.localScale = new Vector3(1, 1, 1);
    }
    else if (moveInput < 0)
    {
        facingRight = false;
        transform.localScale = new Vector3(-1, 1, 1);
    }

    if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && isGrounded)
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        anim.SetTrigger("Jump");
        isGrounded = false;
        anim.SetBool("isMoving", false); // ✅ Stop running animation when jumping
    }

    if (hasWeapon && Input.GetMouseButtonDown(0) && Time.time >= nextFireTime)
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

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;
        Vector2 shootDirection = (mousePosition - firePoint.position).normalized;
        float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.Euler(0, 0, angle - 90));

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
        anim.SetBool("isGrounded", true);

        // ✅ Reset "Jump" trigger so it doesn't block transitions
        anim.ResetTrigger("Jump");

        // ✅ Update "isMoving" immediately to avoid getting stuck in Idle
        float moveInput = Input.GetAxisRaw("Horizontal");
        bool isMoving = Mathf.Abs(moveInput) > 0.01f;
        anim.SetBool("isMoving", isMoving);
    }
}


    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            anim.SetBool("isGrounded", false);
        }
    }

    // **Weapon Pickup Trigger**
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("TranquilizerPickUp"))
        {
            hasWeapon = true;
            Destroy(collision.gameObject); // Remove the weapon pickup
        }
    }
}
