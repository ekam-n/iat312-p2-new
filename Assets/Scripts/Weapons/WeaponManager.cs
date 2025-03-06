using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public Weapon currentWeapon;      // Currently equipped weapon
    public Transform flamethrowerHolder;    // The transform where the weapon is attached
    public Transform blowdartHolder;
    private bool weaponHeld = false; // checks if weapon held
    private bool mouseBehind = false; // checks if mouse is behind player
    public SpriteRenderer playerSprite; // Reference to the player's SpriteRenderer to check facing direction
    private Animator anim;

    void Start() {

        anim = GetComponent<Animator>();
    }

    void Update()
    {

        Vector2 playerPos = transform.position;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (playerSprite.flipX) {

            if (mousePos.x < playerPos.x) mouseBehind = false;
            else mouseBehind = true;
        }

        else {

            if (mousePos.x < playerPos.x) mouseBehind = true;
            else mouseBehind = false;
        }

        // Process the weapon's input if one is equipped
        if (currentWeapon != null)
        {
            currentWeapon.HandleInput();
        }

        anim.SetBool("weaponHeld", weaponHeld);
        anim.SetBool("mouseBehind", mouseBehind);
    }

    // Equip a new weapon
    public void EquipWeapon(Weapon newWeapon)
    {
        if (currentWeapon != null)
        {
            currentWeapon.OnUnequip();
        }

        weaponHeld = true;
        currentWeapon = newWeapon;
        // currentWeapon.transform.SetParent(flamethrowerHolder);
        // currentWeapon.transform.localPosition = Vector3.zero;
        currentWeapon.OnEquip();
    }

    // Unequip the current weapon
    public void UnequipWeapon()
    {
        if (currentWeapon != null)
        {
            currentWeapon.OnUnequip();
            currentWeapon = null;
            weaponHeld = false;
        }
    }
}
