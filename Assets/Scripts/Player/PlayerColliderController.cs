using UnityEngine;

public class PlayerColliderController : MonoBehaviour
{
    public PlayerMovement thePlayer;
    public BoxCollider2D playerCol;

    [SerializeField] Vector2 standoffSet, standSize;
    [SerializeField] Vector2 crouchoffset, crouchSize;

    void Start()
    {
        thePlayer = GetComponent<PlayerMovement>();
        standSize = playerCol.size;
        standoffSet = playerCol.offset;

    }

    void Update()
    {
        if (thePlayer.isCrouched)
        {
            playerCol.size = crouchSize;
            playerCol.offset = crouchoffset;
        }

        if (!thePlayer.isCrouched)
        {
            playerCol.size = standSize;
            playerCol.offset = standoffSet;
        }
    }
}
