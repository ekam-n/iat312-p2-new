using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private float currSpeed = 0f;
    public Transform groundCheck;       // Assign in Inspector: the GroundCheck object
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;       // Set this to the layer(s) that represent your ground

    public float jumpForce = 20f;
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer spriteRenderer; // reference to SpriteRenderer
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // ✅ Get SpriteRenderer Component
    }

    void Update()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        currSpeed = Mathf.Abs(rb.linearVelocity.x); // Also make sure you use rb.velocity.x
        anim.SetFloat("currSpeed", currSpeed);

        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        if (moveInput != 0)
        {
            spriteRenderer.flipX = moveInput < 0;
        }


        // Jumping
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false;
            anim.SetBool("isGrounded", false);
            anim.SetTrigger("jumpTrigger");
        }
    }


    void FixedUpdate()
    {
        // Check for ground using OverlapCircle
        bool grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        if (grounded != isGrounded) // Only update if there is a change
        {
            isGrounded = grounded;
            anim.SetBool("isGrounded", isGrounded);
            anim.ResetTrigger("jumpTrigger");
        }
    }

    // private void OnCollisionEnter2D(Collision2D collision)
    // {
    //     if (collision.gameObject.CompareTag("Ground"))
    //     {
    //         isGrounded = true;
    //         anim.SetBool("isGrounded", true);
    //     }
    // }

    // private void OnCollisionExit2D(Collision2D collision)
    // {
    //     if (collision.gameObject.CompareTag("Ground"))
    //     {
    //         isGrounded = false;
    //         anim.SetBool("isGrounded", false);
    //     }
    // }
}
