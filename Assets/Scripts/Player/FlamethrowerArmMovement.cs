using UnityEngine;

public class FlamethrowerArmMovement : MonoBehaviour
{
    public SpriteRenderer playerSprite; // Reference to the player's SpriteRenderer.
    private SpriteRenderer armSprite;     // Reference to this arm's SpriteRenderer.
    public float angleOffset = 0f;        // Adjust based on your flamethrower arm's default orientation.
    
    // Reference to the central weapon switcher.
    public PlayerWeaponSwitcher weaponSwitcher;

    // Local flag to indicate if the flamethrower is equipped.
    private bool flamethrowerHeld;

    void Start()
    {
        armSprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Query the weapon switcher for the flamethrower status.
        flamethrowerHeld = (weaponSwitcher != null && weaponSwitcher.IsFlamethrowerEquipped);
        Debug.Log("FlamethrowerArmMovement: flamethrowerHeld = " + flamethrowerHeld);

        // Enable or disable the arm sprite based on whether the flamethrower is equipped.
        if (!flamethrowerHeld)
        {
            armSprite.enabled = false;
            return;
        }
        else
        {
            armSprite.enabled = true;
        }

        // Get the arm's pivot and mouse positions.
        Vector2 armPos = transform.position;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Calculate the angle from the arm's pivot to the mouse.
        float angle = Mathf.Atan2(mousePos.y - armPos.y, mousePos.x - armPos.x) * Mathf.Rad2Deg;
        angle += angleOffset;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // Flip the arm horizontally if the mouse is to the left of the player's center.
        if (mousePos.x < playerSprite.transform.position.x)
            armSprite.flipX = true;
        else
            armSprite.flipX = false;
    }
}
