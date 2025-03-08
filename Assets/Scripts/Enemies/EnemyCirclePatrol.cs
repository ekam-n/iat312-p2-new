using UnityEngine;

public class EnemyCircularPatrol : MonoBehaviour
{
    public float radius = 3f;  // Adjust the size of the circular path
    public float speed = 2f;   // Adjust how fast the enemy moves

    private Vector3 centerPosition;
    private float angle = 0f;

    void Start()
    {
        centerPosition = transform.position; // Set the center point of the circle
    }

    void Update()
    {
        MoveInCircle();
    }

    void MoveInCircle()
    {
        angle += speed * Time.deltaTime; // Increase angle over time
        float x = Mathf.Cos(angle) * radius;
        float y = Mathf.Sin(angle) * radius;
        transform.position = centerPosition + new Vector3(x, y, 0);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius); // Show the circular path in Scene View
    }
}
