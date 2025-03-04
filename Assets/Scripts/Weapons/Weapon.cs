using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    // Called when the weapon is equipped
    public abstract void OnEquip();

    // Called when the weapon is unequipped
    public abstract void OnUnequip();

    // Called every frame while the weapon is active
    public abstract void HandleInput();
}
