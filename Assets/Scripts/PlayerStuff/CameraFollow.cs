using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float FollowSpeed = 5f;
    public Transform target;

    private void Update()
    {
        Vector3 newPos = new Vector3(target.position.x, target.position.y, -10f);
        transform.position = Vector3.Slerp(transform.position, newPos, FollowSpeed*Time.deltaTime);
    }

}
