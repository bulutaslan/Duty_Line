using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisualSingle : MonoBehaviour
{
    // Bir grid h�cresinin g�rselli�ini temsil eden s�n�ft�r

    [SerializeField] private MeshRenderer meshRenderer;   // Bu grid h�cresinin g�rseli i�in MeshRenderer

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
