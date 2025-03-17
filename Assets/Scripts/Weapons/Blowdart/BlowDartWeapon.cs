using UnityEngine;

public class BlowDartWeapon : Weapon
{
    public GameObject dartPrefab;         // Normal dart prefab
    public GameObject poisonDartPrefab;   // Poison dart prefab
    public Transform shootPoint;          // The point where the dart is spawned.
    public float dartSpeed = 15f;         // Speed of the dart projectile.
    public Transform playerTransform;     // Reference to the player's transform.
    public float gravityScale = 1f;       // Gravity scale to affect the dart trajectory

    private Vector3 originalShootPointLocalPos;  // To store the original shootPoint local position.
    private SpriteRenderer sr;                   // Weapon's SpriteRenderer reference.

    public int normalDartAmmo = 0;      // Track normal dart ammo
    public int poisonDartAmmo = 0;      // Track poison dart ammo

    public int startingNormalDarts = 10; // Set your starting normal dart ammo here
    public int startingPoisonDarts = 5;  // Set your starting poison dart ammo here

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
        // Shooting normal dart on left click
        if (Input.GetMouseButtonDown(0) && normalDartAmmo > 0)
        {
            ShootDart(false); // False for normal dart
        }

        // Shooting poison dart on right click
        if (Input.GetMouseButtonDown(1) && poisonDartAmmo > 0)
        {
            ShootDart(true);  // True for poison dart
        }
    }

    public void ShootDart(bool isPoison)
    {
        Debug.Log("Attempting to shoot dart...");  // Debug log added
        GameObject dartToShoot = isPoison ? poisonDartPrefab : dartPrefab;
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Set z to 0 since we're working in 2D

        // Get direction towards mouse
        Vector3 shootDirection = (mousePosition - shootPoint.position).normalized;

        // Instantiate the dart and set it on its way
        GameObject dart = Instantiate(dartToShoot, shootPoint.position, Quaternion.identity);
        Rigidbody2D rb = dart.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = shootDirection * dartSpeed;
            rb.gravityScale = gravityScale; // Apply gravity to the dart
        }

        // Update ammo count
        if (isPoison)
            poisonDartAmmo--;
        else
            normalDartAmmo--;

        Debug.Log($"Normal Darts Left: {normalDartAmmo} | Poison Darts Left: {poisonDartAmmo}");
    }

    public void AddNormalDarts(int amount)
    {
        normalDartAmmo = Mathf.Min(normalDartAmmo + amount, startingNormalDarts);
    }

    public void AddPoisonDarts(int amount)
    {
        poisonDartAmmo = Mathf.Min(poisonDartAmmo + amount, startingPoisonDarts);
    }

    // New method to reset ammo
    public void ResetAmmo()
    {
        normalDartAmmo = 0; // Reset to starting normal darts amount
        poisonDartAmmo = 0; // Reset to starting poison darts amount
    }
}
