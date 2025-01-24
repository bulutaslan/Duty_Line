using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ScreenShake : MonoBehaviour
{
    // Ekran sars�nt�s�n� kontrol eden s�n�f

    public static ScreenShake Instance { get; private set; }

    private CinemachineImpulseSource cinemachineImpulseSource;  // Cinemachine'� kullanarak sars�nt� etkisi olu�turmak i�in gereken kayna��

    private void Awake()
    {
        // Singleton pattern: Ayn� nesneden sadece bir tane olmas�n� sa�lar
        if (Instance != null)
        {
            Debug.LogError("There's more than one ScreenShake! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;

        cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
    }

    // Ekran sars�nt�s� olu�turma fonksiyonu
    public void Shake(float intensity = 1f)
    {
        cinemachineImpulseSource.GenerateImpulse(intensity);   // Sars�nt� etkisini olu�turur
    }
}
