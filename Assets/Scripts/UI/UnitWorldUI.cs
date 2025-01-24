using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitWorldUI : MonoBehaviour
{
    // Bir birimin d�nya �zerindeki kullan�c� aray�z�n� y�neten s�n�f

    [SerializeField] private TextMeshProUGUI actionPointsText;   // Aksiyon puan�n� g�stermek i�in metin
    [SerializeField] private Unit unit;   // UI's�n� y�netti�i birim
    [SerializeField] private Image healthBarImage;   // Sa�l�k bar�n�n g�sterece�i g�rsel
    [SerializeField] private HealthSystem healthSystem;   // Sa�l�k sistemi

    private void Start()
    {
        Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;   // Aksiyon puan� de�i�imlerini dinle
        healthSystem.OnDamaged += HealthSystem_OnDamaged;   // Hasar olay�n� dinle

        UpdateActionPointsText();   // Aksiyon puan� metnini g�ncelle
        UpdateHealthBar();   // Sa�l�k bar�n� g�ncelle
    }

    private void UpdateActionPointsText()
    {
        // Aksiyon puan�n� metin olarak g�nceller
        actionPointsText.text = unit.GetActionPoints().ToString();
    }

    private void Unit_OnAnyActionPointsChanged(object sender, EventArgs e)
    {
        UpdateActionPointsText();   // Aksiyon puan� de�i�ti�inde g�ncelle
    }

    private void UpdateHealthBar()
    {
        // Sa�l�k bar�n�n dolulu�unu g�nceller
        healthBarImage.fillAmount = healthSystem.GetHealthNormalized();
    }

    private void HealthSystem_OnDamaged(object sender, EventArgs e)
    {
        UpdateHealthBar();   // Hasar ald���nda sa�l�k bar�n� g�ncelle
    }
}
