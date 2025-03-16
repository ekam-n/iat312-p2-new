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

    private int fireballAmmo = 0;
    public int maxFireballs = 5;
    
    private int normalDartAmmo = 0;
    private int poisonDartAmmo = 0;
    public int maxNormalDarts = 10;
    public int maxPoisonDarts = 5;

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

    public bool UseFireball()
    {
        if (fireballAmmo > 0)
        {
            fireballAmmo--;
            return true;
        }
        return false;
    }

    // Add this method to reset ammo
    public void ResetAmmo()
    {
        fireballAmmo = 0;
        normalDartAmmo = 0;
        poisonDartAmmo = 0;

    // Optionally reset weapon states if needed
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
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("PoisonDartAmmo"))
        {
            AddPoisonDarts(2);
            Destroy(other.gameObject);
        }
    }

    public int GetFireballAmmo() => fireballAmmo;
    public int GetNormalDartAmmo() => normalDartAmmo;
    public int GetPoisonDartAmmo() => poisonDartAmmo;
}
