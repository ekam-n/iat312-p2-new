using UnityEngine;

public class BlowDartWeapon : Weapon
{
    public GameObject dartPrefab;     // Assign your dart projectile prefab in the Inspector.
    public Transform shootPoint;      // The point where the dart is spawned.
    public float dartSpeed = 15f;       // Speed of the dart projectile.

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
        // For example, shoot a dart when left mouse button is pressed.
        if (Input.GetMouseButtonDown(0))
        {
            ShootDart();
        }
    }

    void ShootDart()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - (Vector2)shootPoint.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        // Instantiate the dart so that its forward direction points along the shot direction.
        GameObject dart = Instantiate(dartPrefab, shootPoint.position, Quaternion.Euler(0, 0, angle));
        Rigidbody2D rb = dart.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = direction * dartSpeed;
        }
    }
}
