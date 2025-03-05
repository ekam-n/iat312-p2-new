using UnityEngine;

public class PlayerWeaponSwitcher : MonoBehaviour
{
    public WeaponManager weaponManager;  // Assign the WeaponManager from your player in the Inspector
    public Flamethrower flamethrowerPrefab; // Assign your Flamethrower prefab in the Inspector

    void Update()
    {
        // For example, press the "F" key to equip the flamethrower
        if (Input.GetKeyDown(KeyCode.F))
        {
            EquipFlamethrower();
        }
    }

    void EquipFlamethrower()
    {
        // Instantiate the flamethrower prefab at the position of the weaponHolder
        Flamethrower newFlamethrower = Instantiate(flamethrowerPrefab, weaponManager.weaponHolder.position, Quaternion.identity);
        
        // Equip it using the WeaponManager. This will set the parent to weaponHolder and reset its local position.
        weaponManager.EquipWeapon(newFlamethrower);
    }
}
