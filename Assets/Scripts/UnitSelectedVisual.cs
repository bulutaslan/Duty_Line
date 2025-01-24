using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelectedVisual : MonoBehaviour
{
    // Se�ili birim vizyonunu g�steren sistem

    [SerializeField] private Unit unit;   // Se�ili birim

    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;   // Se�ilen birim de�i�imi olay�na abone ol

        UpdateVisual();   // G�rseli g�ncelle
    }

    // Se�ilen birim de�i�ti�inde
    private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs empty)
    {
        UpdateVisual();
    }

    // G�rsel durumu g�nceller
    private void UpdateVisual()
    {
        if (UnitActionSystem.Instance.GetSelectedUnit() == unit)
        {
            meshRenderer.enabled = true;   // Se�ili birim ise g�rseli aktif et
        }
        else
        {
            meshRenderer.enabled = false;   // Se�ili birim de�ilse g�rseli kapat
        }
    }

    private void OnDestroy()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged -= UnitActionSystem_OnSelectedUnitChanged;   // Olaydan ��k
    }
}
