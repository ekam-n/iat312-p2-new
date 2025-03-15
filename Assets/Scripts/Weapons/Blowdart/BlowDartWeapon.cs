using UnityEngine;
using UnityEngine.UIElements;

public class BlowDartWeapon : Weapon
{
    public GameObject dartPrefab;         // Assign your dart projectile prefab in the Inspector.
    public GameObject poisonDartPrefab;
    public Transform shootPoint;          // The point where the dart is spawned.
    public float dartSpeed = 15f;           // Speed of the dart projectile.
    public Transform playerTransform;     // Reference to the player's transform.

    private Vector3 originalShootPointLocalPos;  // To store the original shootPoint local position.
    private SpriteRenderer sr;                   // Weapon's SpriteRenderer reference.

    private int normalDartAmmo = 0;      // Track normal dart ammo
    private int poisonDartAmmo = 0;      // Track poison dart ammo

    public override void OnEquip()
    {
        gameObject.SetActive(true);
        if (shootPoint != null)
        {
            originalShootPointLocalPos = shootPoint.localPosition;
        }
        sr = GetComponent<SpriteRenderer>();
    }

    public override void OnUnequip()
    {
        gameObject.SetActive(false);
    }

    public override void HandleInput()
    {
        // Rotate the weapon toward the mouse.
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 weaponPos = transform.position;
        float angle = Mathf.Atan2(mousePos.y - weaponPos.y, mousePos.x - weaponPos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // Determine if the mouse is to the left of the player.
        bool flipVertically = false;
        if (playerTransform != null)
        {
            if (mousePos.x < playerTransform.position.x)
                flipVertically = true;
        }
        else if (transform.parent != null)
        {
            if (mousePos.x < transform.parent.position.x)
                flipVertically = true;
        }
        
        // Flip the weapon's sprite vertically based on the condition.
        if (sr != null)
        {
            sr.flipY = flipVertically;
        }

        // Adjust the shootPoint's local position so the dart spawns from the correct side.
        if (shootPoint != null)
        {
            if (flipVertically)
                shootPoint.localPosition = new Vector3(originalShootPointLocalPos.x, -originalShootPointLocalPos.y, originalShootPointLocalPos.z);
            else
                shootPoint.localPosition = originalShootPointLocalPos;
        }

        // Fire a normal dart when left mouse button is pressed (if ammo is available)
        if (Input.GetMouseButtonDown(0) && normalDartAmmo > 0)
        {
            ShootDart();
            normalDartAmmo--;
            Debug.Log("Normal dart fired. Remaining: " + normalDartAmmo);
        }
        else if (Input.GetMouseButtonDown(0) && normalDartAmmo <= 0)
        {
            Debug.Log("Out of normal darts!");
        }

        // Fire a poison dart when right mouse button is pressed (if ammo is available)
        if (Input.GetMouseButtonDown(1) && poisonDartAmmo > 0)
        {
            ShootPoisonDart();
            poisonDartAmmo--;
            Debug.Log("Poison dart fired. Remaining: " + poisonDartAmmo);
        }
        else if (Input.GetMouseButtonDown(1) && poisonDartAmmo <= 0)
        {
            Debug.Log("Out of poison darts!");
        }
    }

    void ShootDart()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - (Vector2)shootPoint.position).normalized;
        float dartAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        // Instantiate the dart with rotation so its forward direction aligns with the shot.
        GameObject dart = Instantiate(dartPrefab, shootPoint.position, Quaternion.Euler(0, 0, dartAngle));
        Rigidbody2D rb = dart.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = direction * dartSpeed;
        }
    }

    void ShootPoisonDart()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - (Vector2)shootPoint.position).normalized;
        float dartAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        // Instantiate the dart with rotation so its forward direction aligns with the shot.
        GameObject dart = Instantiate(poisonDartPrefab, shootPoint.position, Quaternion.Euler(0, 0, dartAngle));
        Rigidbody2D rb = dart.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = direction * dartSpeed;
        }
    }

    // Method to add normal dart ammo
    public void AddNormalDarts(int amount)
    {
        normalDartAmmo += amount;
        Debug.Log("Added " + amount + " normal darts. Total: " + normalDartAmmo);
    }

    // Method to add poison dart ammo
    public void AddPoisonDarts(int amount)
    {
        poisonDartAmmo += amount;
        Debug.Log("Added " + amount + " poison darts. Total: " + poisonDartAmmo);
    }

    // Method to get current normal dart ammo count
    public int GetNormalDartAmmo()
    {
        return normalDartAmmo;
    }

    // Method to get current poison dart ammo count
    public int GetPoisonDartAmmo()
    {
        return poisonDartAmmo;
    }
}