using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ScreenShake : MonoBehaviour
{
    // Ekran sarsýntýsýný kontrol eden sýnýf

    public static ScreenShake Instance { get; private set; }

    private CinemachineImpulseSource cinemachineImpulseSource;  // Cinemachine'ý kullanarak sarsýntý etkisi oluþturmak için gereken kaynaðý

    private void Awake()
    {
        // Singleton pattern: Ayný nesneden sadece bir tane olmasýný saðlar
        if (Instance != null)
        {
            Debug.LogError("There's more than one ScreenShake! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;

        cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
    }

    // Ekran sarsýntýsý oluþturma fonksiyonu
    public void Shake(float intensity = 1f)
    {
        cinemachineImpulseSource.GenerateImpulse(intensity);   // Sarsýntý etkisini oluþturur
    }
}
