using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerAimAndShoot : MonoBehaviour
{
    [SerializeField] private GameObject gun;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject flame;

    [SerializeField] private Transform bulletSpawnPoint;

    private GameObject bulletInst;

    private Vector2 worldPos;
    private Vector2 direction;
    private float angle;
    private float timer = 0f;
    private float interval = 0.25f;



    private void Update()
    {
        GunRot();
        GunShoot();

    }

    private void GunRot()
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
            if (timer >= interval)
            {
                timer = 0f;
                bulletInst = Instantiate(bullet, bulletSpawnPoint.position, gun.transform.rotation);
            }

        }

        if (Mouse.current.rightButton.isPressed)
        {
            bulletInst = Instantiate(flame, bulletSpawnPoint.position, gun.transform.rotation);
        }


    }

}