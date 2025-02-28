using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerAimAndShoot : MonoBehaviour
{
    [SerializeField] private GameObject gun;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject flame;
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

        Vector3 loScale = new Vector3(0.05f, 0.05f, 1f);
        if (angle > 90 || angle < -90)
        {
            loScale.y *= -1f;
        }
        else
        {
            loScale.y *= 1f;
        }

        gun.transform.localScale = loScale;
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
            bulletInst = Instantiate(flame, bulletSpawnPoint.position, gun.transform.rotation);
        }

        else if (Mouse.current.rightButton.isPressed)
        {
            timer += Time.deltaTime;
            if (timer >= flameInterval)
            {
                timer = 0f;
                bulletInst = Instantiate(flame, bulletSpawnPoint.position, gun.transform.rotation);
            }
        }

        if (Keyboard.current.leftShiftKey.wasPressedThisFrame)
        {
            bulletInst = Instantiate(bomb, bulletSpawnPoint.position, gun.transform.rotation);
        }

    }

}