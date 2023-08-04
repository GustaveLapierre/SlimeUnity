using UnityEngine;

public class PersistObject : MonoBehaviour
{
    public static PersistObject Instance;

    public string difficulty;
    public string username;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
}