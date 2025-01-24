using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitActionSystemUI : MonoBehaviour
{
    // Bu sýnýf, oyun içindeki bir birimin aksiyon sistemini kullanýcý arayüzünde göstermek için kullanýlýr.

    [SerializeField] private Transform actionButtonPrefab;   // Aksiyon butonlarýnýn þablonu
    [SerializeField] private Transform actionButtonContainerTransform;   // Aksiyon butonlarýnýn yerleþtirileceði transform
    [SerializeField] private TextMeshProUGUI actionPointsText;   // Aksiyon puaný göstermek için metin

    private List<ActionButtonUI> actionButtonUIList;   // Kullanýcý arayüzündeki aksiyon butonlarý listesi

    private void Awake()
    {
        actionButtonUIList = new List<ActionButtonUI>();
    }

    private void Start()
    {
        // Aksiyon sistemindeki olaylarý dinleyerek ilgili iþlemleri baþlatýr
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
        UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
        UnitActionSystem.Instance.OnActionStarted += UnitActionSystem_OnActionStarted;
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;

        UpdateActionPoints();   // Aksiyon puanýný güncelle
        CreateUnitActionButtons();   // Birim aksiyon butonlarýný oluþtur
        UpdateSelectedVisual();   // Seçili görseli güncelle
    }

    private void CreateUnitActionButtons()
    {
        // Eski butonlarý kaldýr
        foreach (Transform buttonTransform in actionButtonContainerTransform)
        {
            Destroy(buttonTransform.gameObject);
        }

        actionButtonUIList.Clear();

        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();   // Seçili birimi al

        // Seçili birimin sahip olduðu aksiyonlarý butonlara dönüþtür
        foreach (BaseAction baseAction in selectedUnit.GetBaseActionArray())
        {
            Transform actionButtonTransform = Instantiate(actionButtonPrefab, actionButtonContainerTransform);
            ActionButtonUI actionButtonUI = actionButtonTransform.GetComponent<ActionButtonUI>();
            actionButtonUI.SetBaseAction(baseAction);

            actionButtonUIList.Add(actionButtonUI);   // Butonu listeye ekle
        }
    }

    private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs e)
    {
        CreateUnitActionButtons();   // Birim seçimi deðiþtiðinde butonlarý yeniden oluþtur
        UpdateSelectedVisual();   // Seçili görseli güncelle
        UpdateActionPoints();   // Aksiyon puanýný güncelle
    }

    private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e)
    {
        UpdateSelectedVisual();   // Seçili görseli güncelle
    }

    private void UnitActionSystem_OnActionStarted(object sender, EventArgs e)
    {
        UpdateActionPoints();   // Aksiyon baþlatýldýðýnda puaný güncelle
    }

    private void UpdateSelectedVisual()
    {
        // Her bir aksiyon butonunun seçili olup olmadýðýný güncelle
        foreach (ActionButtonUI actionButtonUI in actionButtonUIList)
        {
            actionButtonUI.UpdateSelectedVisual();
        }
    }

    private void UpdateActionPoints()
    {
        // Seçili birimin sahip olduðu aksiyon puanýný günceller
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();

        actionPointsText.text = "Action Points: " + selectedUnit.GetActionPoints();   // Aksiyon puaný metnini güncelle
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        UpdateActionPoints();   // Sýra deðiþtiðinde aksiyon puanýný güncelle
    }

    private void Unit_OnAnyActionPointsChanged(object sender, EventArgs e)
    {
        UpdateActionPoints();   // Birimin aksiyon puaný deðiþtiðinde güncelle
    }
}
