using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 20f;
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer spriteRenderer; // ✅ Reference to SpriteRenderer
    private bool isGrounded;
    private bool isMoving;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // ✅ Get SpriteRenderer Component
    }

    void Update()
    {
        float moveInput = Input.GetAxisRaw("Horizontal"); // Supports A/D and Arrow Keys
        isMoving = moveInput != 0;
        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isGrounded", isGrounded);

        // Update Rigidbody movement
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);


        // 🔄 Best Way to Flip the Player Sprite
        if (moveInput > 0) 
        {
            spriteRenderer.flipX = false; // Face Right
        }
        else if (moveInput < 0)
        {
            spriteRenderer.flipX = true; // Face Left
        }

        // Jumping
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            // anim.SetTrigger("Jump");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            anim.SetBool("isGrounded", true);
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
}
