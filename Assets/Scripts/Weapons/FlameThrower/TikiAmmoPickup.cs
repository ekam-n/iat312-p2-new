using UnityEngine;
public class TikiAmmoPickup : MonoBehaviour
{
    public int fireballAmount = 3;
    private Vector3 initialPosition;

    private void Start() { initialPosition = transform.position; }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerWeaponSwitcher player = other.GetComponent<PlayerWeaponSwitcher>();
            if (player != null)
            {
                player.AddFireballs(fireballAmount);
                gameObject.SetActive(false);
            }
        }
    }

    public void ResetPickup()
    {
        transform.position = initialPosition;
        gameObject.SetActive(true);
    }
}