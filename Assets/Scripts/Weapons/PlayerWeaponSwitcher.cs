using UnityEngine;

public class PlayerWeaponSwitcher : MonoBehaviour
{
    public WeaponManager weaponManager;
    public Flamethrower flamethrowerPrefab;
    public BlowDartWeapon blowDartPrefab;

    public Flamethrower flamethrowerInstance;
    private BlowDartWeapon blowDartInstance;

    private bool canEquipFlamethrower = false;
    private bool canEquipBlowDart = false;
    private bool isFlamethrowerActive = false;
    private bool isBlowDartActive = false;

    private int fireballAmmo = 0; // Start with zero fireballs
    public int maxFireballs = 10; // Set max fireball capacity
    
    public int normalDartAmmo = 0; // Start with zero normal darts
    public int poisonDartAmmo = 0; // Start with zero poison darts
    public int maxNormalDarts = 10; // Set max normal dart capacity
    public int maxPoisonDarts = 5; // Set max poison dart capacity

    public bool IsFlamethrowerEquipped => weaponManager.currentWeapon is Flamethrower;
    public bool IsBlowDartEquipped => weaponManager.currentWeapon is BlowDartWeapon;

    void Start()
    {
        if (flamethrowerPrefab != null && weaponManager != null)
        {
            flamethrowerInstance = Instantiate(flamethrowerPrefab, weaponManager.flamethrowerHolder.position, Quaternion.identity, weaponManager.flamethrowerHolder);
            flamethrowerInstance.transform.localPosition = Vector3.zero;
            flamethrowerInstance.transform.localRotation = Quaternion.Euler(0, 0, -90);
            flamethrowerInstance.gameObject.SetActive(false);
            flamethrowerInstance.playerTransform = transform;
        }

        if (blowDartPrefab != null && weaponManager != null)
        {
            blowDartInstance = Instantiate(blowDartPrefab, weaponManager.blowdartHolder.position, Quaternion.identity, weaponManager.blowdartHolder);
            blowDartInstance.transform.localPosition = Vector3.zero;
            blowDartInstance.transform.localRotation = Quaternion.identity;
            blowDartInstance.gameObject.SetActive(false);
            blowDartInstance.playerTransform = transform;
        }
    }

    void Update()
    {
        if (canEquipFlamethrower && Input.GetKeyDown(KeyCode.F))
        {
            ToggleFlamethrower();
        }

        if (canEquipBlowDart && Input.GetKeyDown(KeyCode.B))
        {
            ToggleBlowDart();
        }

        // Handle input for weapon actions
        if (IsBlowDartEquipped)
        {
            // Handle Dart Shooting (Left Click for normal dart, Right Click for poison dart)
            if (Input.GetMouseButtonDown(0) && blowDartInstance.normalDartAmmo > 0) // Left click for normal dart
            {
                blowDartInstance.ShootDart(false); // False for normal dart
            }
            else if (Input.GetMouseButtonDown(1) && blowDartInstance.poisonDartAmmo > 0) // Right click for poison dart
            {
                blowDartInstance.ShootDart(true); // True for poison dart
            }
        }

        // Fireball usage input (press a key, for example, spacebar)
        if (IsFlamethrowerEquipped && fireballAmmo > 0 && Input.GetKeyDown(KeyCode.Space)) // Example: using fireballs when pressing Space
        {
            UseFireball();
        }

        Debug.Log("Fireballs: " + fireballAmmo + " | Normal Darts: " + normalDartAmmo + " | Poison Darts: " + poisonDartAmmo);
    }

    void ToggleFlamethrower()
    {
        if (isBlowDartActive)
        {
            ToggleBlowDart();
        }

        if (flamethrowerInstance != null)
        {
            isFlamethrowerActive = !isFlamethrowerActive;
            flamethrowerInstance.gameObject.SetActive(isFlamethrowerActive);

            if (isFlamethrowerActive)
            {
                weaponManager.EquipWeapon(flamethrowerInstance);
                flamethrowerInstance.AddFireballs(fireballAmmo);
            }
            else
            {
                weaponManager.UnequipWeapon();
            }
        }
    }

    void ToggleBlowDart()
    {
        Debug.Log("Toggling BlowDart");  // Debug log added
        if (isFlamethrowerActive)
        {
            ToggleFlamethrower();
        }

        if (blowDartInstance != null)
        {
            isBlowDartActive = !isBlowDartActive;
            blowDartInstance.gameObject.SetActive(isBlowDartActive);

            if (isBlowDartActive)
            {
                weaponManager.EquipWeapon(blowDartInstance);
                blowDartInstance.AddNormalDarts(normalDartAmmo);  // Sync normal darts
                blowDartInstance.AddPoisonDarts(poisonDartAmmo);  // Sync poison darts
            }
            else
            {
                weaponManager.UnequipWeapon();
            }
        }
    }

    // Fireball usage method
    public bool UseFireball()
    {
        if (fireballAmmo > 0)
        {
            fireballAmmo--;
            Debug.Log("Fireball used! Remaining: " + fireballAmmo);
            // Optionally, trigger the flamethrower's fireball effect here if needed.
            // flamethrowerInstance.Fire(); // Uncomment if there's a Fire method in the Flamethrower class
            return true;
        }
        else
        {
            Debug.Log("Out of fireballs!");
            return false;
        }
    }

    public void AddFireballs(int amount)
    {
        fireballAmmo = Mathf.Min(fireballAmmo + amount, maxFireballs);
        if (IsFlamethrowerEquipped && flamethrowerInstance != null)
        {
            flamethrowerInstance.AddFireballs(amount);
        }
    }

    public void AddNormalDarts(int amount)
    {
        normalDartAmmo = Mathf.Min(normalDartAmmo + amount, maxNormalDarts);
        if (IsBlowDartEquipped && blowDartInstance != null)
        {
            blowDartInstance.AddNormalDarts(amount);
        }
    }

    public void AddPoisonDarts(int amount)
    {
        poisonDartAmmo = Mathf.Min(poisonDartAmmo + amount, maxPoisonDarts);
        if (IsBlowDartEquipped && blowDartInstance != null)
        {
            blowDartInstance.AddPoisonDarts(amount);
        }
    }

    // Reset ammo to default values
    public void ResetAmmo()
{
    fireballAmmo = 0;  // Reset fireball ammo
    normalDartAmmo = 0;  // Reset normal dart ammo
    poisonDartAmmo = 0;  // Reset poison dart ammo

    // Reset BlowDartWeapon ammo
    if (IsBlowDartEquipped && blowDartInstance != null)
    {
        blowDartInstance.ResetAmmo();  // Call the ResetAmmo method in BlowDartWeapon
    }

    Debug.Log("Ammo reset: Fireballs = " + fireballAmmo + ", Normal Darts = " + normalDartAmmo + ", Poison Darts = " + poisonDartAmmo);
}


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("FlamethrowerPickup"))
        {
            canEquipFlamethrower = true;
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("BlowdartPickup"))
        {
            canEquipBlowDart = true;
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("NormalDartAmmo"))
        {
            AddNormalDarts(5);
            other.gameObject.SetActive(false);  // Hide instead of destroy
        }
        else if (other.CompareTag("PoisonDartAmmo"))
        {
            AddPoisonDarts(2);
            other.gameObject.SetActive(false);  // Hide instead of destroy
        }
    }

    // Methods to get the current ammo counts
    public int GetFireballAmmo()
    {
        return fireballAmmo;
    }
    
    public int GetNormalDartAmmo()
    {
        return normalDartAmmo;
    }

    public int GetPoisonDartAmmo()
    {
        return poisonDartAmmo;
    }
}
