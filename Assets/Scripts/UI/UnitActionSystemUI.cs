using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitActionSystemUI : MonoBehaviour
{
    // Bu s�n�f, oyun i�indeki bir birimin aksiyon sistemini kullan�c� aray�z�nde g�stermek i�in kullan�l�r.

    [SerializeField] private Transform actionButtonPrefab;   // Aksiyon butonlar�n�n �ablonu
    [SerializeField] private Transform actionButtonContainerTransform;   // Aksiyon butonlar�n�n yerle�tirilece�i transform
    [SerializeField] private TextMeshProUGUI actionPointsText;   // Aksiyon puan� g�stermek i�in metin

    private List<ActionButtonUI> actionButtonUIList;   // Kullan�c� aray�z�ndeki aksiyon butonlar� listesi

    private void Awake()
    {
        actionButtonUIList = new List<ActionButtonUI>();
    }

    private void Start()
    {
        // Aksiyon sistemindeki olaylar� dinleyerek ilgili i�lemleri ba�lat�r
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
        UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
        UnitActionSystem.Instance.OnActionStarted += UnitActionSystem_OnActionStarted;
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;

        UpdateActionPoints();   // Aksiyon puan�n� g�ncelle
        CreateUnitActionButtons();   // Birim aksiyon butonlar�n� olu�tur
        UpdateSelectedVisual();   // Se�ili g�rseli g�ncelle
    }

    private void CreateUnitActionButtons()
    {
        // Eski butonlar� kald�r
        foreach (Transform buttonTransform in actionButtonContainerTransform)
        {
            Destroy(buttonTransform.gameObject);
        }

        actionButtonUIList.Clear();

        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();   // Se�ili birimi al

        // Se�ili birimin sahip oldu�u aksiyonlar� butonlara d�n��t�r
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
        CreateUnitActionButtons();   // Birim se�imi de�i�ti�inde butonlar� yeniden olu�tur
        UpdateSelectedVisual();   // Se�ili g�rseli g�ncelle
        UpdateActionPoints();   // Aksiyon puan�n� g�ncelle
    }

    private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e)
    {
        UpdateSelectedVisual();   // Se�ili g�rseli g�ncelle
    }

    private void UnitActionSystem_OnActionStarted(object sender, EventArgs e)
    {
        UpdateActionPoints();   // Aksiyon ba�lat�ld���nda puan� g�ncelle
    }

    private void UpdateSelectedVisual()
    {
        // Her bir aksiyon butonunun se�ili olup olmad���n� g�ncelle
        foreach (ActionButtonUI actionButtonUI in actionButtonUIList)
        {
            actionButtonUI.UpdateSelectedVisual();
        }
    }

    private void UpdateActionPoints()
    {
        // Se�ili birimin sahip oldu�u aksiyon puan�n� g�nceller
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();

        actionPointsText.text = "Action Points: " + selectedUnit.GetActionPoints();   // Aksiyon puan� metnini g�ncelle
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        UpdateActionPoints();   // S�ra de�i�ti�inde aksiyon puan�n� g�ncelle
    }

    private void Unit_OnAnyActionPointsChanged(object sender, EventArgs e)
    {
        UpdateActionPoints();   // Birimin aksiyon puan� de�i�ti�inde g�ncelle
    }
}
