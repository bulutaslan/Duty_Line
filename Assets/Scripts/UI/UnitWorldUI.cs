using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitWorldUI : MonoBehaviour
{
    // Bir birimin dünya üzerindeki kullanýcý arayüzünü yöneten sýnýf

    [SerializeField] private TextMeshProUGUI actionPointsText;   // Aksiyon puanýný göstermek için metin
    [SerializeField] private Unit unit;   // UI'sýný yönettiði birim
    [SerializeField] private Image healthBarImage;   // Saðlýk barýnýn göstereceði görsel
    [SerializeField] private HealthSystem healthSystem;   // Saðlýk sistemi

    private void Start()
    {
        Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;   // Aksiyon puaný deðiþimlerini dinle
        healthSystem.OnDamaged += HealthSystem_OnDamaged;   // Hasar olayýný dinle

        UpdateActionPointsText();   // Aksiyon puaný metnini güncelle
        UpdateHealthBar();   // Saðlýk barýný güncelle
    }

    private void UpdateActionPointsText()
    {
        // Aksiyon puanýný metin olarak günceller
        actionPointsText.text = unit.GetActionPoints().ToString();
    }

    private void Unit_OnAnyActionPointsChanged(object sender, EventArgs e)
    {
        UpdateActionPointsText();   // Aksiyon puaný deðiþtiðinde güncelle
    }

    private void UpdateHealthBar()
    {
        // Saðlýk barýnýn doluluðunu günceller
        healthBarImage.fillAmount = healthSystem.GetHealthNormalized();
    }

    private void HealthSystem_OnDamaged(object sender, EventArgs e)
    {
        UpdateHealthBar();   // Hasar aldýðýnda saðlýk barýný güncelle
    }
}
