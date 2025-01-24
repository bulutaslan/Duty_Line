using UnityEngine;

public class ActionCamera : MonoBehaviour
{

    public static ActionCamera Instance { get; private set; }

    private void Start()
    {
        // Singleton kontrolü
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Fazlalýk olan ActionCamera yok et
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Sahneler arasý taþýnabilir hale getir
    }
    private void Awake()
    {
        // Singleton kontrolü
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Fazlalýk olan ActionCamera yok et
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Sahneler arasý taþýnabilir hale getir
    }
}
