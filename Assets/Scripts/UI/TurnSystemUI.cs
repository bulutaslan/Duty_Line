using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TurnSystemUI : MonoBehaviour
{
    // Sýra sistemi UI'sýný yöneten sýnýf

    [SerializeField] private Button endTurnBtn;   // Sona erdir butonu
    [SerializeField] private TextMeshProUGUI turnNumberText;   // Tur numarasýný gösterecek metin
    [SerializeField] private GameObject enemyTurnVisualGameObject;   // Düþman turunun göstergesi

    private void Start()
    {
        endTurnBtn.onClick.AddListener(() =>
        {
            // Sona erdir butonuna týklandýðýnda sýrayý deðiþtir
            TurnSystem.Instance.NextTurn();
        });

        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;

        UpdateTurnText();   // Tur metnini güncelle
        UpdateEnemyTurnVisual();   // Düþman tur göstergesini güncelle
        UpdateEndTurnButtonVisibility();   // Sona erdir butonunun görünümünü güncelle
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        // Sýra deðiþtiðinde UI öðelerini güncelle
        UpdateTurnText();
        UpdateEnemyTurnVisual();
        UpdateEndTurnButtonVisibility();
    }

    private void UpdateTurnText()
    {
        // Tur numarasýný günceller
        turnNumberText.text = "TURN " + TurnSystem.Instance.GetTurnNumber();
    }

    private void UpdateEnemyTurnVisual()
    {
        // Düþman turu göstergesini günceller
        enemyTurnVisualGameObject.SetActive(!TurnSystem.Instance.IsPlayerTurn());
    }

    private void UpdateEndTurnButtonVisibility()
    {
        // Sona erdir butonunun görünürlüðünü günceller
        endTurnBtn.gameObject.SetActive(TurnSystem.Instance.IsPlayerTurn());
    }
}
