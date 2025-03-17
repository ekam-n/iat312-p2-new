using UnityEngine;
using System.Collections; // This imports IEnumerator


public class CoroutineHandler : MonoBehaviour
{
    public static CoroutineHandler Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Coroutine RunCoroutine(IEnumerator routine)
    {
        return StartCoroutine(routine); // Returns Coroutine that can be used if needed
    }
}
