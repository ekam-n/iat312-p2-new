using UnityEngine;

public class AmmoManager : MonoBehaviour
{
    public static AmmoManager instance; // Singleton

    public int fireballAmmo { get; private set; } = 0;
    public int normalDartAmmo { get; private set; } = 0;
    public int poisonDartAmmo { get; private set; } = 0;

    public int maxFireballs = 5;
    public int maxNormalDarts = 10;
    public int maxPoisonDarts = 5;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddFireballs(int amount)
    {
        fireballAmmo = Mathf.Min(fireballAmmo + amount, maxFireballs);
        Debug.Log("Added Fireballs: " + fireballAmmo);
    }

    public void AddNormalDarts(int amount)
    {
        normalDartAmmo = Mathf.Min(normalDartAmmo + amount, maxNormalDarts);
        Debug.Log("Added Normal Darts: " + normalDartAmmo);
    }

    public void AddPoisonDarts(int amount)
    {
        poisonDartAmmo = Mathf.Min(poisonDartAmmo + amount, maxPoisonDarts);
        Debug.Log("Added Poison Darts: " + poisonDartAmmo);
    }

    public void ResetAmmo()
    {
        fireballAmmo = 0;
        normalDartAmmo = 0;
        poisonDartAmmo = 0;
        Debug.Log("All ammo reset to 0.");
    }
}
