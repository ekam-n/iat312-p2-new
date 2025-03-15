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
    
    private int normalDartAmmo = 0; // Start with zero normal darts
    private int poisonDartAmmo = 0; // Start with zero poison darts
    public int maxNormalDarts = 10; // Set max normal dart capacity
    public int maxPoisonDarts = 5; // Set max poison dart capacity

    public bool IsFlamethrowerEquipped => weaponManager.currentWeapon is Flamethrower;
    public bool IsBlowDartEquipped => weaponManager.currentWeapon is BlowDartWeapon;

    void Start()
    {
        // Instantiate flamethrower but keep it inactive
        if (flamethrowerPrefab != null && weaponManager != null)
        {
            flamethrowerInstance = Instantiate(flamethrowerPrefab, weaponManager.flamethrowerHolder.position, Quaternion.identity, weaponManager.flamethrowerHolder);
            flamethrowerInstance.transform.localPosition = Vector3.zero;
            flamethrowerInstance.transform.localRotation = Quaternion.Euler(0, 0, -90);
            flamethrowerInstance.gameObject.SetActive(false);
            
            // Set player transform reference to the flamethrower
            flamethrowerInstance.playerTransform = transform;
        }

        // Instantiate blow dart weapon but keep it inactive
        if (blowDartPrefab != null && weaponManager != null)
        {
            blowDartInstance = Instantiate(blowDartPrefab, weaponManager.blowdartHolder.position, Quaternion.identity, weaponManager.blowdartHolder);
            blowDartInstance.transform.localPosition = Vector3.zero;
            blowDartInstance.transform.localRotation = Quaternion.identity;
            blowDartInstance.gameObject.SetActive(false);
            
            // Set player transform reference to the blowdart
            blowDartInstance.playerTransform = transform;
        }
    }

    void Update()
    {
        // Press F to toggle the flamethrower (only if the player has picked it up)
        if (canEquipFlamethrower && Input.GetKeyDown(KeyCode.F))
        {
            ToggleFlamethrower();
        }

        // Press B to toggle the blow dart weapon (only if the player has picked it up)
        if (canEquipBlowDart && Input.GetKeyDown(KeyCode.B))
        {
            ToggleBlowDart();
        }

        // For debugging, log ammo counts
        Debug.Log("Fireballs: " + fireballAmmo + " | Normal Darts: " + normalDartAmmo + " | Poison Darts: " + poisonDartAmmo);
    }

    void ToggleFlamethrower()
    {
        // If blowdart is active, unequip it first
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
                // If the flamethrower is equipped, pass fireball ammo to the flamethrower instance
                if (flamethrowerInstance != null)
                {
                    flamethrowerInstance.AddFireballs(fireballAmmo); // Pass the current fireball ammo
                }
            }
            else
            {
                weaponManager.UnequipWeapon();
            }
        }
    }

    void ToggleBlowDart()
    {
        // If flamethrower is active, unequip it first
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
                // Pass the current ammo counts to the blowdart instance
                blowDartInstance.AddNormalDarts(normalDartAmmo);
                blowDartInstance.AddPoisonDarts(poisonDartAmmo);
            }
            else
            {
                weaponManager.UnequipWeapon();
            }
        }
    }

    public void AddFireballs(int amount)
    {
        fireballAmmo = Mathf.Min(fireballAmmo + amount, maxFireballs);
        Debug.Log("Picked up TikiAmmo! Fireballs: " + fireballAmmo);

        // If flamethrower is equipped, update its ammo as well.
        if (IsFlamethrowerEquipped && flamethrowerInstance != null)
        {
            flamethrowerInstance.AddFireballs(amount); // Update flamethrower's ammo count
        }
    }

    public void AddNormalDarts(int amount)
    {
        normalDartAmmo = Mathf.Min(normalDartAmmo + amount, maxNormalDarts);
        Debug.Log("Picked up Normal Dart Ammo! Normal Darts: " + normalDartAmmo);

        // If blowdart is equipped, update its ammo as well
        if (IsBlowDartEquipped && blowDartInstance != null)
        {
            blowDartInstance.AddNormalDarts(amount);
        }
    }

    public void AddPoisonDarts(int amount)
    {
        poisonDartAmmo = Mathf.Min(poisonDartAmmo + amount, maxPoisonDarts);
        Debug.Log("Picked up Poison Dart Ammo! Poison Darts: " + poisonDartAmmo);

        // If blowdart is equipped, update its ammo as well
        if (IsBlowDartEquipped && blowDartInstance != null)
        {
            blowDartInstance.AddPoisonDarts(amount);
        }
    }

    public bool UseFireball()
    {
        if (fireballAmmo > 0)
        {
            fireballAmmo--;
            Debug.Log("Fireball used. Remaining: " + fireballAmmo);
            return true; // Successfully used a fireball
        }
        Debug.Log("Out of fireballs!");
        return false; // No fireballs left
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("FlamethrowerPickup"))
        {
            canEquipFlamethrower = true;
            Debug.Log("Flamethrower Pickup Collected!");
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("BlowdartPickup"))
        {
            canEquipBlowDart = true;
            Debug.Log("Blowdart Pickup Collected!");
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("NormalDartAmmo"))
        {
            // You can set different amounts for different pickups if needed
            AddNormalDarts(5);
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("PoisonDartAmmo"))
        {
            // You can set different amounts for different pickups if needed
            AddPoisonDarts(2);
            Destroy(other.gameObject);
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