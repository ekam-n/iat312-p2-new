using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public float patrolDistance = 5f;  // Set the patrol range in Unity Inspector
    public float speed = 2f;
    public float detectionRange = 5f;  // Distance at which enemy starts chasing
    public float chaseRange = 7f;      // Distance at which enemy stops chasing
    public float chaseSpeed = 3f;      // Speed when chasing the player
    public float damage = 20f;         // Damage amount when colliding with player

    private Vector3 startPosition;
    private Vector3 leftLimit, rightLimit;
    private bool movingRight = true;
    private Transform player;
    private bool isChasing = false;

    void Start()
    {
        startPosition = transform.position;
        leftLimit = startPosition + Vector3.left * patrolDistance;
        rightLimit = startPosition + Vector3.right * patrolDistance;
        player = GameObject.FindGameObjectWithTag("Player")?.transform; // Find the player by tag
    }

    void Update()
    {
        if (player == null) return; // Ensure there's a player before continuing

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer < detectionRange)
        {
            isChasing = true; // Start chasing if the player enters detection range
        }
        else if (distanceToPlayer > chaseRange)
        {
            isChasing = false; // Stop chasing if the player moves out of chase range
        }

        if (isChasing)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        transform.position += (movingRight ? Vector3.right : Vector3.left) * speed * Time.deltaTime;

        if (transform.position.x >= rightLimit.x) movingRight = false;
        if (transform.position.x <= leftLimit.x) movingRight = true;
    }

    void ChasePlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + Vector3.left * patrolDistance, transform.position + Vector3.right * patrolDistance);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRange); // Show detection range
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, chaseRange); // Show chase stopping range
    }
}
