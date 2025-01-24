using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Oyunu yeniden ba�latma fonksiyonu
    public void RestartGame()
    {
        // Sahne de�i�ikliklerini takiben DontDestroyOnLoad nesnelerini temizleyin

        Time.timeScale = 1f;
        // Mevcut sahneyi yeniden y�kle
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }
}
