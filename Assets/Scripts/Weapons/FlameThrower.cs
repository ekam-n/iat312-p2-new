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
        // Left mouse button held: enable and update continuous flame effect
        if (Input.GetMouseButton(0))
        {
            if (flameEffectInstance != null && !flameEffectInstance.activeSelf)
                flameEffectInstance.SetActive(true);

            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (mousePos - (Vector2)shootPoint.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            flameEffectInstance.transform.rotation = Quaternion.Euler(0, 0, angle);

            // Optionally adjust the localScale to reflect flameRange:
            // flameEffectInstance.transform.localScale = new Vector3(flameRange, 1, 1);
        }
        else
        {
            if (flameEffectInstance != null && flameEffectInstance.activeSelf)
                flameEffectInstance.SetActive(false);
        }


        // Right mouse button pressed: fire a fireball as before
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
