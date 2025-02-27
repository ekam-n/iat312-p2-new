using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    [SerializeField] private float normalBulletSpeed = 15f;
    [SerializeField] private float flameBulletSpeed = 0f;
    [SerializeField] private float normalDestroyTime = 3f;
    [SerializeField] private float flameDestroyTime = 0.03f;
    [SerializeField] private LayerMask whatDestroysBullets;

    public enum BulletType
    {
        bullet,
        flame
    }

    public BulletType bulletType;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        SetDestroyTime();

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
    }
}