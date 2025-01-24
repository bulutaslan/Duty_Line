using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TurnSystemUI : MonoBehaviour
{
    // S�ra sistemi UI's�n� y�neten s�n�f

    [SerializeField] private Button endTurnBtn;   // Sona erdir butonu
    [SerializeField] private TextMeshProUGUI turnNumberText;   // Tur numaras�n� g�sterecek metin
    [SerializeField] private GameObject enemyTurnVisualGameObject;   // D��man turunun g�stergesi

    private void Start()
    {
        endTurnBtn.onClick.AddListener(() =>
        {
            // Sona erdir butonuna t�kland���nda s�ray� de�i�tir
            TurnSystem.Instance.NextTurn();
        });

        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;

        UpdateTurnText();   // Tur metnini g�ncelle
        UpdateEnemyTurnVisual();   // D��man tur g�stergesini g�ncelle
        UpdateEndTurnButtonVisibility();   // Sona erdir butonunun g�r�n�m�n� g�ncelle
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        // S�ra de�i�ti�inde UI ��elerini g�ncelle
        UpdateTurnText();
        UpdateEnemyTurnVisual();
        UpdateEndTurnButtonVisibility();
    }

    private void UpdateTurnText()
    {
        // Tur numaras�n� g�nceller
        turnNumberText.text = "TURN " + TurnSystem.Instance.GetTurnNumber();
    }

    private void UpdateEnemyTurnVisual()
    {
        // D��man turu g�stergesini g�nceller
        enemyTurnVisualGameObject.SetActive(!TurnSystem.Instance.IsPlayerTurn());
    }

    private void UpdateEndTurnButtonVisibility()
    {
        // Sona erdir butonunun g�r�n�rl���n� g�nceller
        endTurnBtn.gameObject.SetActive(TurnSystem.Instance.IsPlayerTurn());
    }
}
