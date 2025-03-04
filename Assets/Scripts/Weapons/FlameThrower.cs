using UnityEngine;

public class Flamethrower : Weapon
{
    [Header("Projectile Prefabs")]
    public GameObject flamePrefab;    // Prefab for the flame projectile
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
    }

    public override void OnUnequip()
    {
        gameObject.SetActive(false);
    }

    public override void HandleInput()
    {
        // Left mouse button held: Continuous flame shooting
        if (Input.GetMouseButton(0))
        {
            flameTimer += Time.deltaTime;
            if (flameTimer >= flameFireRate)
            {
                ShootFlame();
                flameTimer = 0f;
            }
        }
        else
        {
            // Reset the timer when not shooting continuously
            flameTimer = flameFireRate;
        }

        // Right mouse button pressed: Single fireball shot
        if (Input.GetMouseButtonDown(1))
        {
            ShootFireball();
        }
    }

    void ShootFlame()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - (Vector2)shootPoint.position).normalized;

        GameObject flame = Instantiate(flamePrefab, shootPoint.position, Quaternion.identity);
        Rigidbody2D rb = flame.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = direction * flameSpeed;
        }
        // Optionally, you can add logic for flame lifetime or damage here.
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
