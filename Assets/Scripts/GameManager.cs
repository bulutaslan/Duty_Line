using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Oyunu yeniden baþlatma fonksiyonu
    public void RestartGame()
    {
        // Sahne deðiþikliklerini takiben DontDestroyOnLoad nesnelerini temizleyin

        Time.timeScale = 1f;
        // Mevcut sahneyi yeniden yükle
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }
}
