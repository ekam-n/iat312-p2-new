using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public Weapon currentWeapon;      // Currently equipped weapon
    public Transform weaponHolder;    // The transform where the weapon is attached
    public SpriteRenderer playerSprite; // Reference to the player's SpriteRenderer to check facing direction

    void Update()
    {
        if (playerSprite.flipX)
        {
            // Flip the x coordinate to move it to the left.
            weaponHolder.localPosition = new Vector3(-Mathf.Abs(weaponHolder.localPosition.x), weaponHolder.localPosition.y, weaponHolder.localPosition.z);
        }
        else
        {
            // Ensure it’s positive when facing right.
            weaponHolder.localPosition = new Vector3(Mathf.Abs(weaponHolder.localPosition.x), weaponHolder.localPosition.y, weaponHolder.localPosition.z);
        }


        // Process the weapon's input if one is equipped
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
