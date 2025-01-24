using UnityEngine;

public class ActionCamera : MonoBehaviour
{

    public static ActionCamera Instance { get; private set; }

    private void Start()
    {
        // Singleton kontrol�
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Fazlal�k olan ActionCamera yok et
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Sahneler aras� ta��nabilir hale getir
    }
    private void Awake()
    {
        // Singleton kontrol�
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Fazlal�k olan ActionCamera yok et
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Sahneler aras� ta��nabilir hale getir
    }
}
