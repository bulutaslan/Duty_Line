using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    // S�ra tabanl� sistemin y�netimini sa�layan s�n�f

    public static TurnSystem Instance { get; private set; }

    public event EventHandler OnTurnChanged;   // S�ra de�i�ti�inde tetiklenecek olay

    private int turnNumber = 1;   // D�ng�n�n ba�lang�c�ndaki s�ra numaras�
    private bool isPlayerTurn = true;   // Oyuncunun s�rada olup olmad���n� belirler

    private void Awake()
    {
        // Singleton pattern: Ayn� nesneden sadece bir tane olmas�n� sa�lar
        if (Instance != null)
        {
            Debug.LogError("There's more than one TurnSystem! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // Sonraki s�ra de�i�ikli�i fonksiyonu
    public void NextTurn()
    {
        turnNumber++;
        isPlayerTurn = !isPlayerTurn;   // S�ra oyuncudan kar�� tarafa ge�iyor

        OnTurnChanged?.Invoke(this, EventArgs.Empty);   // S�ra de�i�ti�inde tetiklenen olay
    }

    public int GetTurnNumber()
    {
        return turnNumber;   // S�ra numaras�n� d�nd�r�r
    }

    public bool IsPlayerTurn()
    {
        return isPlayerTurn;   // Oyuncunun s�rada olup olmad���n� d�nd�r�r
    }
}
