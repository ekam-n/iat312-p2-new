using UnityEngine;

public class EnemyCircularPatrol : MonoBehaviour
{
    public float radius = 3f;         // Size of circular patrol path
    public float speed = 2f;          // Patrol movement speed
    public float detectionRange = 5f; // Distance at which enemy starts chasing
    public float chaseRange = 7f;     // Distance at which enemy stops chasing
    public float chaseSpeed = 3f;     // Speed when chasing the player
    public float damage = 20f;        // Damage amount when colliding with player

    private Vector3 centerPosition;
    private float angle = 0f;
    private Transform player;
    private bool isChasing = false;

    void Start()
    {
        centerPosition = transform.position; // Set the center of the patrol path
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
            MoveInCircle();
        }
    }

    void MoveInCircle()
    {
        angle += speed * Time.deltaTime; // Increase angle over time
        float x = Mathf.Cos(angle) * radius;
        float y = Mathf.Sin(angle) * radius;
        transform.position = centerPosition + new Vector3(x, y, 0);
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
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius); // Show patrol path
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRange); // Show detection range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange); // Show chase stopping range
    }
}
