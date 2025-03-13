using UnityEngine;

public class PlayerWeaponSwitcher : MonoBehaviour
{
    public WeaponManager weaponManager;
    public Flamethrower flamethrowerPrefab;
    public BlowDartWeapon blowDartPrefab;

    public Flamethrower flamethrowerInstance;
    private BlowDartWeapon blowDartInstance;

    private bool canEquipFlamethrower = false;
    private bool isFlamethrowerActive = false;

    private int fireballAmmo = 0; // Start with zero fireballs
    public int maxFireballs = 5; // Set max fireball capacity

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
        }

        // Instantiate blow dart weapon but keep it inactive
        if (blowDartPrefab != null && weaponManager != null)
        {
            blowDartInstance = Instantiate(blowDartPrefab, weaponManager.blowdartHolder.position, Quaternion.identity, weaponManager.blowdartHolder);
            blowDartInstance.transform.localPosition = Vector3.zero;
            blowDartInstance.transform.localRotation = Quaternion.identity;
            blowDartInstance.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        // Press F to toggle the flamethrower (only if the player has picked it up)
        if (canEquipFlamethrower && Input.GetKeyDown(KeyCode.F))
        {
            ToggleFlamethrower();
        }

        // Press B to equip the blow dart weapon
        if (Input.GetKeyDown(KeyCode.B))
        {
            EquipBlowDart();
        }

        // For debugging, log ammo count
        Debug.Log("Fireballs: " + fireballAmmo);
    }

    void ToggleFlamethrower()
    {
        if (flamethrowerInstance != null)
        {
            isFlamethrowerActive = !isFlamethrowerActive;
            flamethrowerInstance.gameObject.SetActive(isFlamethrowerActive);

            if (isFlamethrowerActive)
            {
                weaponManager.EquipWeapon(flamethrowerInstance);
            }
            else
            {
                weaponManager.UnequipWeapon();
            }
        }
    }

    void EquipBlowDart()
    {
        if (blowDartInstance != null && !blowDartInstance.gameObject.activeInHierarchy)
        {
            blowDartInstance.gameObject.SetActive(true);
            weaponManager.EquipWeapon(blowDartInstance);
        }
    }

    public void AddFireballs(int amount)
    {
        fireballAmmo = Mathf.Min(fireballAmmo + amount, maxFireballs);
        Debug.Log("Picked up TikiAmmo! Fireballs: " + fireballAmmo);
        
        // If flamethrower is equipped, update its ammo as well.
        if (IsFlamethrowerEquipped && flamethrowerInstance != null)
        {
            flamethrowerInstance.AddFireballs(amount); // Add ammo to Flamethrower
        }
    }

    public bool UseFireball()
    {
        if (fireballAmmo > 0)
        {
            fireballAmmo--;
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
    }

    // You can also add a method to get the current fireball count if needed:
    public int GetFireballAmmo()
    {
        return fireballAmmo;
    }
}
