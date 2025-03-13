using UnityEngine;

public class PlayerWeaponSwitcher : MonoBehaviour
{
    public WeaponManager weaponManager;      // Assign the WeaponManager from your player in the Inspector
    public Flamethrower flamethrowerPrefab;  // Assign your Flamethrower prefab in the Inspector
    public BlowDartWeapon blowDartPrefab;    // Assign your BlowDartWeapon prefab in the Inspector

    private Flamethrower flamethrowerInstance;
    private BlowDartWeapon blowDartInstance;

    // Flag to track whether the flamethrower can be equipped
    private bool canEquipFlamethrower = false;
    private bool isFlamethrowerActive = false; // Track whether the flamethrower is currently equipped

    // Public properties to check which weapon is currently equipped.
    public bool IsFlamethrowerEquipped
    {
        get { return weaponManager.currentWeapon is Flamethrower; }
    }

    public bool IsBlowDartEquipped
    {
        get { return weaponManager.currentWeapon is BlowDartWeapon; }
    }

    void Start()
    {
        // Pre-instantiate the flamethrower at the flamethrowerHolder's position,
        // parent it to the flamethrowerHolder, reset its local transform,
        // rotate it 90° clockwise, and then deactivate it.
        if (flamethrowerPrefab != null && weaponManager != null)
        {
            flamethrowerInstance = Instantiate(flamethrowerPrefab, weaponManager.flamethrowerHolder.position, Quaternion.identity, weaponManager.flamethrowerHolder);
            flamethrowerInstance.transform.localPosition = Vector3.zero;
            // Rotate 90 degrees clockwise (i.e., -90 degrees)
            flamethrowerInstance.transform.localRotation = Quaternion.Euler(0, 0, -90);
            flamethrowerInstance.gameObject.SetActive(false);
        }

        // Pre-instantiate the blow dart weapon at the blowdartHolder's position,
        // parent it to the blowdartHolder, reset its local transform, and then deactivate it.
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
        // Press F to toggle the flamethrower, but only if the player can equip it.
        if (canEquipFlamethrower && Input.GetKeyDown(KeyCode.F))
        {
            ToggleFlamethrower();
        }

        // Press B to equip the blow dart weapon.
        if (Input.GetKeyDown(KeyCode.B))
        {
            EquipBlowDart();
        }

        // For debugging, log the currently equipped weapon type.
        if (weaponManager.currentWeapon != null)
        {
            Debug.Log("Equipped weapon: " + weaponManager.currentWeapon.GetType().Name);
        }
    }

    void ToggleFlamethrower()
    {
        if (flamethrowerInstance != null)
        {
            isFlamethrowerActive = !isFlamethrowerActive; // Toggle state
            flamethrowerInstance.gameObject.SetActive(isFlamethrowerActive);
            
            if (isFlamethrowerActive)
            {
                weaponManager.EquipWeapon(flamethrowerInstance);
            }
            else
            {
                weaponManager.UnequipWeapon(); // Ensure this method exists to unequip weapons
            }
        }
    }

    void EquipBlowDart()
    {
        // If the blow dart weapon is not active, activate and equip it.
        if (blowDartInstance != null && !blowDartInstance.gameObject.activeInHierarchy)
        {
            blowDartInstance.gameObject.SetActive(true);
            weaponManager.EquipWeapon(blowDartInstance);
        }
    }

    // Collision detection for when the player touches the flamethrower pickup
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player collided with the flamethrower pickup
        if (other.CompareTag("FlamethrowerPickup"))
        {
            canEquipFlamethrower = true;  // Allow the player to equip the flamethrower
            Debug.Log("Flamethrower Pickup Collected!");

            // Destroy the pickup object after collection
            Destroy(other.gameObject);  // This destroys the pickup object
        }
    }
}
