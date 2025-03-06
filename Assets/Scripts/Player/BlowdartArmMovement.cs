using UnityEngine;

public class BlowDartArmMovement : MonoBehaviour
{
    public SpriteRenderer playerSprite; // Reference to the player's SpriteRenderer for position info.
    private SpriteRenderer armSprite;     // Reference to this arm's SpriteRenderer.
    public float angleOffset = 0f;        // Adjust based on your blowdart arm's default orientation.
    
    // Reference to the central weapon switcher.
    public PlayerWeaponSwitcher weaponSwitcher;

    // Local flag to indicate if a blow dart is equipped.
    private bool blowdartHeld;

    void Start()
    {
        armSprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Query the weapon switcher for the blow dart status.
        blowdartHeld = (weaponSwitcher != null && weaponSwitcher.IsBlowDartEquipped);
        Debug.Log("BlowDartArmMovement: blowdartHeld = " + blowdartHeld);

        // Enable or disable the arm sprite based on whether the blow dart is equipped.
        if (!blowdartHeld)
        {
            armSprite.enabled = false;
            return;
        }
        else
        {
            armSprite.enabled = true;
        }

        // Get the arm's position and the mouse position.
        Vector2 armPos = transform.position;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        // Calculate the angle from the arm's pivot to the mouse.
        float angle = Mathf.Atan2(mousePos.y - armPos.y, mousePos.x - armPos.x) * Mathf.Rad2Deg;
        angle += angleOffset;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // Flip the arm vertically if the mouse is to the left of the player's center.
        if (mousePos.x < playerSprite.transform.position.x)
            armSprite.flipY = true;
        else
            armSprite.flipY = false;
    }
}
