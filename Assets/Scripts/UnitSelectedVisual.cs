using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelectedVisual : MonoBehaviour
{
    // Seçili birim vizyonunu gösteren sistem

    [SerializeField] private Unit unit;   // Seçili birim

    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;   // Seçilen birim deðiþimi olayýna abone ol

        UpdateVisual();   // Görseli güncelle
    }

    // Seçilen birim deðiþtiðinde
    private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs empty)
    {
        UpdateVisual();
    }

    // Görsel durumu günceller
    private void UpdateVisual()
    {
        if (UnitActionSystem.Instance.GetSelectedUnit() == unit)
        {
            meshRenderer.enabled = true;   // Seçili birim ise görseli aktif et
        }
        else
        {
            meshRenderer.enabled = false;   // Seçili birim deðilse görseli kapat
        }
    }

    private void OnDestroy()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged -= UnitActionSystem_OnSelectedUnitChanged;   // Olaydan çýk
    }
}
