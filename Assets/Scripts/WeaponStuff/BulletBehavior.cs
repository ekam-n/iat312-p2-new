using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    [SerializeField] private float normalBulletSpeed = 35f;
    [SerializeField] private float flameBulletSpeed = 35f;
    [SerializeField] private float bombBulletSpeed = 35f;

    [SerializeField] private float normalDestroyTime = 3f;
    [SerializeField] private float flameDestroyTime = 0.03f;
    [SerializeField] private float bombDestroyTime = 5f;

    [SerializeField] private float bombPhysicsValue = 3f;

    [SerializeField] private LayerMask whatDestroysBullets;

    public enum BulletType
    {
        bullet,
        flame,
        bomb
    }

    public BulletType bulletType;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        SetDestroyTime();
        SetRBStats();
        InitializeBulletStats();
    }

    private void InitializeBulletStats()
    {
        if (bulletType == BulletType.bullet)
        {
            SetStraightVelocity();
        }

        else if (bulletType == BulletType.flame)
        {
            SetFlameVelocity();
        }

        else if (bulletType == BulletType.bomb)
        {
            SetBombVelocity();
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((whatDestroysBullets.value & (1 << collision.gameObject.layer)) > 0)
        {
            //a
            Destroy(gameObject);
        }
    }

    private void SetStraightVelocity()
    {
        rb.linearVelocity = transform.right * normalBulletSpeed;
    }

    private void SetFlameVelocity()
    {
        rb.linearVelocity = transform.right * flameBulletSpeed;
    }

    private void SetBombVelocity()
    {
        rb.linearVelocity = transform.right * bombBulletSpeed;
    }

    private void SetDestroyTime()
    {
        if (bulletType == BulletType.bullet)
        {
            Destroy(gameObject, normalDestroyTime);
        }

        else if (bulletType == BulletType.flame)
        {
            Destroy(gameObject, flameDestroyTime);
        }

        else if (bulletType == BulletType.bomb)
        {
            Destroy(gameObject, bombDestroyTime);
        }
    }

    private void SetRBStats()
    {
        if (bulletType == BulletType.bullet)
        {
            rb.gravityScale = 0f;
        }

        else if (bulletType == BulletType.flame)
        {
            rb.gravityScale = 0f;
        }

        else if (bulletType == BulletType.bomb)
        {
            rb.gravityScale = bombPhysicsValue;
        }
    }
}