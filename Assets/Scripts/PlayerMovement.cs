using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 20f;
    private Rigidbody2D rb;
    private Animator anim;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>(); // Get Animator Component
    }

    void Update()
    {
        // Movement (Left/Right) - Supports Arrow Keys and A/D
        float moveInput = Input.GetAxisRaw("Horizontal"); // Supports A/D and Left/Right Arrows
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // Update Running Animation
        anim.SetBool("isRunning", moveInput != 0); // True when moving, false when idle

        // Jumping with "W" or "Up Arrow"
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            anim.SetTrigger("Jump"); // Trigger jump animation
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if player is touching the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            anim.SetBool("isGrounded", true); // Enable grounded animation
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // When leaving the ground, mark as not grounded
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            anim.SetBool("isGrounded", false); // Disable grounded animation
        }
    }
}
