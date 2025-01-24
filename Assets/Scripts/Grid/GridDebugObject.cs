using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GridDebugObject : MonoBehaviour
{
    // Bu s�n�f, grid sistemi �zerindeki nesneleri g�stermek i�in kullan�l�r

    [SerializeField] private TextMeshPro textMeshPro;   // Grid objesi verisini g�stermek i�in TextMeshPro bile�eni

    private object gridObject;   // Bu grid pozisyonunda bulunan nesne

    // Grid objesini bu metod ile ayarlar
    public virtual void SetGridObject(object gridObject)
    {
        this.gridObject = gridObject;   // Grid nesnesini gridObject de�i�kenine ata
    }

    // Her frame g�ncellenir ve gridObject'�n ToString() metoduyla TextMeshPro �zerinde g�sterilir
    protected virtual void Update()
    {
        textMeshPro.text = gridObject.ToString();   // Grid objesinin metinsel temsilini TextMeshPro bile�enine yazd�r
    }
}
