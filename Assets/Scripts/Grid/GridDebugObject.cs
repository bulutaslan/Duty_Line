using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GridDebugObject : MonoBehaviour
{
    // Bu sýnýf, grid sistemi üzerindeki nesneleri göstermek için kullanýlýr

    [SerializeField] private TextMeshPro textMeshPro;   // Grid objesi verisini göstermek için TextMeshPro bileþeni

    private object gridObject;   // Bu grid pozisyonunda bulunan nesne

    // Grid objesini bu metod ile ayarlar
    public virtual void SetGridObject(object gridObject)
    {
        this.gridObject = gridObject;   // Grid nesnesini gridObject deðiþkenine ata
    }

    // Her frame güncellenir ve gridObject'ýn ToString() metoduyla TextMeshPro üzerinde gösterilir
    protected virtual void Update()
    {
        textMeshPro.text = gridObject.ToString();   // Grid objesinin metinsel temsilini TextMeshPro bileþenine yazdýr
    }
}
