using UnityEngine;

public class PlayerWeaponSwitcher : MonoBehaviour
{
    public WeaponManager weaponManager;      // Assign the WeaponManager from your player in the Inspector
    public Flamethrower flamethrowerPrefab;    // Assign your Flamethrower prefab in the Inspector

    private Flamethrower flamethrowerInstance;

    void Start()
    {
        // Pre-instantiate the flamethrower at the weaponHolder's position,
        // parent it to the weaponHolder, reset its local transform,
        // rotate it 90° clockwise, and then deactivate it.
        if (flamethrowerPrefab != null && weaponManager != null)
        {
            flamethrowerInstance = Instantiate(flamethrowerPrefab, weaponManager.weaponHolder.position, Quaternion.identity, weaponManager.weaponHolder);
            flamethrowerInstance.transform.localPosition = Vector3.zero;
            // Rotate 90 degrees clockwise (i.e., -90 degrees)
            flamethrowerInstance.transform.localRotation = Quaternion.Euler(0, 0, -90);
            flamethrowerInstance.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        // Press F to equip the flamethrower.
        if (Input.GetKeyDown(KeyCode.F))
        {
            EquipFlamethrower();
        }
    }

    void EquipFlamethrower()
    {
        // If the flamethrower is not active, activate and equip it.
        if (flamethrowerInstance != null && !flamethrowerInstance.gameObject.activeInHierarchy)
        {
            flamethrowerInstance.gameObject.SetActive(true);
            weaponManager.EquipWeapon(flamethrowerInstance);
        }
    }
}
