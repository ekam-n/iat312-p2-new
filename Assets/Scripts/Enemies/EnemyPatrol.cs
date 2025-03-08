using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public float patrolDistance = 5f; // Set the patrol range in Unity Inspector
    public float speed = 2f;

    private Vector3 startPosition;
    private Vector3 leftLimit, rightLimit;
    private bool movingRight = true;

    void Start()
    {
        startPosition = transform.position;
        leftLimit = startPosition + Vector3.left * patrolDistance;
        rightLimit = startPosition + Vector3.right * patrolDistance;
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        transform.position += (movingRight ? Vector3.right : Vector3.left) * speed * Time.deltaTime;

        if (transform.position.x >= rightLimit.x) movingRight = false;
        if (transform.position.x <= leftLimit.x) movingRight = true;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + Vector3.left * patrolDistance, transform.position + Vector3.right * patrolDistance);
    }
}
