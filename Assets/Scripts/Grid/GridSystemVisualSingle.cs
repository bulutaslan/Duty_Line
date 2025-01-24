using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisualSingle : MonoBehaviour
{
    // Bir grid hücresinin görselliðini temsil eden sýnýftýr

    [SerializeField] private MeshRenderer meshRenderer;   // Bu grid hücresinin görseli için MeshRenderer

    public void Show(Material material)
    {
        meshRenderer.enabled = true;   // MeshRenderer'i aktif et
        meshRenderer.material = material;   // Belirtilen materyali uygula
    }

    public void Hide()
    {
        meshRenderer.enabled = false;   // MeshRenderer'i kapat
    }
}
