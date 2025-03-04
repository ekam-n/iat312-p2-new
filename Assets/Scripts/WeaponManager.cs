using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public Weapon currentWeapon;      // Reference to the currently equipped weapon
    public Transform weaponHolder;    // A child transform on the player where weapons attach

    void Update()
    {
        // If a weapon is equipped, allow it to process input
        if (currentWeapon != null)
        {
            currentWeapon.HandleInput();
        }
    }

    // Equip a new weapon
    public void EquipWeapon(Weapon newWeapon)
    {
        if (currentWeapon != null)
        {
            currentWeapon.OnUnequip();
        }

        currentWeapon = newWeapon;
        currentWeapon.transform.SetParent(weaponHolder);
        currentWeapon.transform.localPosition = Vector3.zero;
        currentWeapon.OnEquip();
    }

    // Unequip the current weapon
    public void UnequipWeapon()
    {
        if (currentWeapon != null)
        {
            currentWeapon.OnUnequip();
            currentWeapon = null;
        }
    }
}
