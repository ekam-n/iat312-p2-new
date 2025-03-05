using UnityEngine;

public class ArmMovement : MonoBehaviour
{
    public SpriteRenderer playerSprite; // Reference to the player's SpriteRenderer to check facing direction
    private SpriteRenderer armSprite;     // Reference to the arm's SpriteRenderer
    public float angleOffset = 0f;       // Adjust this value based on your sprite's default orientation
    public Animator playerAnim;

    void Start()
    {
        armSprite = GetComponent<SpriteRenderer>();
        // playerAnim = parent.GetComponent<Animator>();
    }

    void Update()
    {

        // Check if a weapon is held using the player's animator parameter.
        bool weaponHeld = playerAnim.GetBool("weaponHeld");
        
        // Instead of disabling the entire GameObject, disable the arm sprite so Update() keeps running.
        if (!weaponHeld)
        {
            if (armSprite.enabled)
                armSprite.enabled = false;
            return;
        }
        else
        {
            if (!armSprite.enabled)
                armSprite.enabled = true;
        }

        // Get the arm pivot's world position
        Vector2 armPos = transform.position;
        // Get the mouse position in world coordinates
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // Compute the raw angle from the arm's pivot to the mouse
        float angle = Mathf.Atan2(mousePos.y - armPos.y, mousePos.x - armPos.x) * Mathf.Rad2Deg;
        
        // Add the offset so the sprite points correctly towards the mouse
        angle += angleOffset;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // Flip the arm sprite if the mouse is to the left of the player's center
        if (mousePos.x < playerSprite.transform.position.x)
        {
            armSprite.flipX = true;
        }
        else
        {
            armSprite.flipX = false;
        }
    }
}
