using UnityEngine;

public class Flamethrower : Weapon
{
    [Header("Projectile Prefabs")]
    [Header("Flame Effect Settings")]
    public GameObject flameEffectPrefab; // Assign your flame effect prefab in the Inspector
    // public float flameRange = 5f;          // Maximum range of the flame
    private GameObject flameEffectInstance;

    public GameObject fireballPrefab; // Prefab for the fireball projectile

    [Header("Shooting Settings")]
    public Transform shootPoint;      // The point where projectiles spawn
    public float flameFireRate = 0.1f;  // Time interval between flame projectiles
    public float flameSpeed = 5f;       // Speed of the flame projectile
    public float fireballSpeed = 10f;   // Speed of the fireball projectile

    private float flameTimer;

    public override void OnEquip()
    {
        gameObject.SetActive(true);
        if (flameEffectPrefab != null && flameEffectInstance == null)
        {
            // Instantiate the flame effect and attach it to shootPoint
            flameEffectInstance = Instantiate(flameEffectPrefab, shootPoint.position, Quaternion.identity, shootPoint);
            flameEffectInstance.transform.localPosition = Vector3.zero;

            flameEffectInstance.SetActive(false); // Start deactivated

        }
    }

    public override void OnUnequip()
    {
        gameObject.SetActive(false);
        if (flameEffectInstance != null)
        {
            flameEffectInstance.SetActive(false);
        }
    }

    public override void HandleInput()
    {
        // Always update the flamethrower rotation to face the mouse
        // Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // Vector2 direction = (mousePos - (Vector2)shootPoint.position).normalized;
        // float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        // transform.rotation = Quaternion.Euler(0, 0, angle);

        // Manage the flame effect: active only when left mouse is held
        if (Input.GetMouseButton(0))
        {
            if (flameEffectInstance != null && !flameEffectInstance.activeSelf)
                flameEffectInstance.SetActive(true);
        }
        else
        {
            if (flameEffectInstance != null && flameEffectInstance.activeSelf)
                flameEffectInstance.SetActive(false);
        }

        // Right mouse button: fire a fireball
        if (Input.GetMouseButtonDown(1))
        {
            ShootFireball();
        }
    }



void ShootFireball()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - (Vector2)shootPoint.position).normalized;

        GameObject fireball = Instantiate(fireballPrefab, shootPoint.position, Quaternion.identity);
        Rigidbody2D rb = fireball.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = direction * fireballSpeed;
        }
        // Optionally, add explosion or damage effects for the fireball.
    }

}
