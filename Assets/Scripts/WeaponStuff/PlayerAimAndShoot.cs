using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerAimAndShoot : MonoBehaviour
{
    [SerializeField] private GameObject gun;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject flame;
    [SerializeField] private Vector2 flameOffset; // Adjustable in Inspector

    [SerializeField] private GameObject bomb;

    [SerializeField] private Transform bulletSpawnPoint;

    private GameObject bulletInst;

    public Vector2 worldPos;
    private Vector2 direction;
    
    private float angle;
    private float timer = 0f;
    private float bulletInterval = 0.25f;
    private float flameInterval = 0.015f;



    private void Update()
    {
        GunRot();
        GunShoot();

    }

   public void GunRot()
   {
        worldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        direction = (worldPos - (Vector2)gun.transform.position).normalized;
        gun.transform.right = direction;

        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Determine player direction (facing right or left)
        bool isFacingLeft = angle > 90 || angle < -90;

        // Flip gun sprite when looking left
        Vector3 loScale = new Vector3(0.05f, 0.05f, 1f);
        loScale.y = isFacingLeft ? -0.05f : 0.05f; // Flip vertically if facing left

        gun.transform.localScale = loScale;

        // Adjust gun position so it stays in front of the player
        float gunOffsetX = isFacingLeft ? -2f : 2f; // Move gun to the left or right
        gun.transform.localPosition = new Vector3(gunOffsetX, gun.transform.localPosition.y, gun.transform.localPosition.z);
    }


    private void GunShoot()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            bulletInst = Instantiate(bullet, bulletSpawnPoint.position, gun.transform.rotation);
        }

        else if (Mouse.current.leftButton.isPressed)
        {
            timer += Time.deltaTime;
            if (timer >= bulletInterval)
            {
                timer = 0f;
                bulletInst = Instantiate(bullet, bulletSpawnPoint.position, gun.transform.rotation);
            }

        }

        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            Vector3 flameSpawnPos = bulletSpawnPoint.position + (gun.transform.right * flameOffset.x) + (gun.transform.up * flameOffset.y);
            bulletInst = Instantiate(flame, flameSpawnPos, gun.transform.rotation);
        }

        else if (Mouse.current.rightButton.isPressed)
        {
            timer += Time.deltaTime;
            if (timer >= flameInterval)
            {
                timer = 0f;
                Vector3 flameSpawnPos = bulletSpawnPoint.position + (gun.transform.right * flameOffset.x) + (gun.transform.up * flameOffset.y);
                bulletInst = Instantiate(flame, flameSpawnPos, gun.transform.rotation);
            }
        }

        if (Keyboard.current.leftShiftKey.wasPressedThisFrame)
        {
            bulletInst = Instantiate(bomb, bulletSpawnPoint.position, gun.transform.rotation);
        }

    }

}