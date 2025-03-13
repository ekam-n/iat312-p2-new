using UnityEngine;
public class Flamethrower : Weapon
{
    [Header("Projectile Prefabs")]
    [Header("Flame Effect Settings")]
    public GameObject flameEffectPrefab; // Assign your flame effect prefab in the Inspector
    private GameObject flameEffectInstance;

    public GameObject fireballPrefab; // Prefab for the fireball projectile

    [Header("Shooting Settings")]
    public Transform shootPoint;      // The point where projectiles spawn
    public float flameFireRate = 0.1f;  // Time interval between flame projectiles
    public float flameSpeed = 5f;       // Speed of the flame projectile
    public float fireballSpeed = 10f;   // Speed of the fireball projectile

    // New: Reference to the player's transform (set in Inspector)
    public Transform playerTransform;

    private float flameTimer;

    // Store original local positions so we can mirror them when needed.
    private Vector3 originalShootPointLocalPos;
    private Vector3 originalFlameEffectLocalPos;

    private int fireballAmmo = 0;  // Keep track of fireball ammo

    public override void OnEquip()
    {
        gameObject.SetActive(true);
        // Store the shootPoint's original local position.
        if (shootPoint != null)
        {
            originalShootPointLocalPos = shootPoint.localPosition;
        }

        if (flameEffectPrefab != null && flameEffectInstance == null)
        {
            // Instantiate the flame effect and attach it to shootPoint
            flameEffectInstance = Instantiate(flameEffectPrefab, shootPoint.position, Quaternion.identity, shootPoint);
            flameEffectInstance.transform.localPosition = Vector3.zero;
            // Store its original local position
            originalFlameEffectLocalPos = flameEffectInstance.transform.localPosition;
            flameEffectInstance.transform.localRotation = Quaternion.identity;
            flameEffectInstance.SetActive(false); // Start deactivated
        }
    }

    public override void OnUnequip()
    {
        gameObject.SetActive(false);
        if (flameEffectInstance != null)
        {
            flameEffectInstance.SetActive(false);
        }
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

        // Get the weapon's SpriteRenderer.
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.flipY = flipVertically;
        }

        // Adjust the local position of shootPoint and flame effect based on flip.
        if (flipVertically)
        {
            // Mirror the y component of the local position.
            if (shootPoint != null)
            {
                shootPoint.localPosition = new Vector3(originalShootPointLocalPos.x, -originalShootPointLocalPos.y, originalShootPointLocalPos.z);
            }
            if (flameEffectInstance != null)
            {
                flameEffectInstance.transform.localPosition = new Vector3(originalFlameEffectLocalPos.x, -originalFlameEffectLocalPos.y, originalFlameEffectLocalPos.z);
            }
        }
        else
        {
            if (shootPoint != null)
            {
                shootPoint.localPosition = originalShootPointLocalPos;
            }
            if (flameEffectInstance != null)
            {
                flameEffectInstance.transform.localPosition = originalFlameEffectLocalPos;
            }
        }

        // Manage the flame effect: activate only when left mouse is held.
        if (Input.GetMouseButton(0))
        {
            if (flameEffectInstance != null && !flameEffectInstance.activeSelf)
                flameEffectInstance.SetActive(true);

            if (flameEffectInstance != null && flameEffectInstance.activeSelf)
            {
                // Ensure the flame effect stays aligned with the shootPoint.
                flameEffectInstance.transform.localRotation = Quaternion.identity;
            }
        }
        else
        {
            if (flameEffectInstance != null && flameEffectInstance.activeSelf)
                flameEffectInstance.SetActive(false);
        }

        // Right mouse button: fire a fireball if ammo is available.
        if (Input.GetMouseButtonDown(1))
        {
            PlayerWeaponSwitcher player = GetComponentInParent<PlayerWeaponSwitcher>();

            if (player != null && player.UseFireball())  // Check ammo in PlayerWeaponSwitcher
            {
                ShootFireball();
                Debug.Log("Fireball shot!");
            }
            else
            {
                Debug.Log("Out of fireball ammo!");
            }
        }
    }

    // AddFireballs method to update ammo count
    public void AddFireballs(int amount)
    {
        fireballAmmo = Mathf.Min(fireballAmmo + amount, 5);  // Max ammo is 5
        Debug.Log("Picked up fireball ammo! Current count: " + fireballAmmo);
    }

    void ShootFireball()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - (Vector2)shootPoint.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        // Instantiate the fireball with the computed rotation
        GameObject fireball = Instantiate(fireballPrefab, shootPoint.position, Quaternion.Euler(0, 0, angle));
        
        // Optionally, set its velocity
        Rigidbody2D rb = fireball.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = direction * fireballSpeed;
        }

        // Decrease fireball ammo after shooting
        fireballAmmo--;
    }
}
