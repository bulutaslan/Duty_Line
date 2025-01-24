using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    // Sýra tabanlý sistemin yönetimini saðlayan sýnýf

    public static TurnSystem Instance { get; private set; }

    public event EventHandler OnTurnChanged;   // Sýra deðiþtiðinde tetiklenecek olay

    private int turnNumber = 1;   // Döngünün baþlangýcýndaki sýra numarasý
    private bool isPlayerTurn = true;   // Oyuncunun sýrada olup olmadýðýný belirler

    private void Awake()
    {
        // Singleton pattern: Ayný nesneden sadece bir tane olmasýný saðlar
        if (Instance != null)
        {
            Debug.LogError("There's more than one TurnSystem! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // Sonraki sýra deðiþikliði fonksiyonu
    public void NextTurn()
    {
        turnNumber++;
        isPlayerTurn = !isPlayerTurn;   // Sýra oyuncudan karþý tarafa geçiyor

        OnTurnChanged?.Invoke(this, EventArgs.Empty);   // Sýra deðiþtiðinde tetiklenen olay
    }

    public int GetTurnNumber()
    {
        return turnNumber;   // Sýra numarasýný döndürür
    }

    public bool IsPlayerTurn()
    {
        return isPlayerTurn;   // Oyuncunun sýrada olup olmadýðýný döndürür
    }
}
