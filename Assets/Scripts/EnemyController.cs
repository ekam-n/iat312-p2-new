using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    public float speed = 3f; // Speed of movement
    public float moveDistance = 3f; // How far up/down to move

    private Rigidbody2D rb;
    private Vector2 startPos;
    private bool movingUp = true;
    private bool isParalyzed = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get Rigidbody2D component
        startPos = transform.position; // Save the starting position
    }

    void FixedUpdate()
    {
        if (!isParalyzed)
        {
            MoveUpAndDown(); // Move the enemy if not paralyzed
        }
        else
        {
            rb.linearVelocity = Vector2.zero; // Stop movement when paralyzed
        }
    }

    void MoveUpAndDown()
    {
        if (movingUp)
        {
            rb.linearVelocity = new Vector2(0, speed); // Move up
            if (transform.position.y >= startPos.y + moveDistance)
            {
                movingUp = false; // Switch direction
            }
        }
        else
        {
            rb.linearVelocity = new Vector2(0, -speed); // Move down
            if (transform.position.y <= startPos.y - moveDistance)
            {
                movingUp = true; // Switch direction
            }
        }
    }

    public void Paralyze(float duration)
    {
        StartCoroutine(ParalyzeCoroutine(duration));
    }

    IEnumerator ParalyzeCoroutine(float duration)
    {
        isParalyzed = true; // Set paralyzed state
        rb.linearVelocity = Vector2.zero; // Stop movement
        yield return new WaitForSeconds(duration); // Wait for the duration
        isParalyzed = false; // Reset paralyzed state
    }
}