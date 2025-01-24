using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject
{
    // Bu s�n�f, bir Grid sistemi �zerinde bir h�crenin sahip oldu�u nesne ve etkile�imleri tan�mlar

    private GridSystem<GridObject> gridSystem;   // Grid sistemi nesnesi
    private GridPosition gridPosition;   // Bu GridObject'�n pozisyonu
    private List<Unit> unitList;   // Bu GridObject �zerinde bulunan birimlerin listesi
    private IInteractable interactable;   // Bu GridObject �zerinde bulunan etkile�im nesnesi

    // Constructor: GridObject, Grid sistemi ve bu grid pozisyonunu al�r
    public GridObject(GridSystem<GridObject> gridSystem, GridPosition gridPosition)
    {
        this.gridSystem = gridSystem;   // Grid sistemi nesnesini ata
        this.gridPosition = gridPosition;   // Grid pozisyonunu ata
        unitList = new List<Unit>();   // Birim listesi ba�lat
    }

    // Bu GridObject'�n ToString() metodu, Grid pozisyonunu ve bu pozisyondaki birimleri d�nd�r�r
    public override string ToString()
    {
        string unitString = "";
        foreach (Unit unit in unitList)
        {
            unitString += unit + "\n";   // Birimlerin metinsel temsillerini birle�tir
        }

        return gridPosition.ToString() + "\n" + unitString;   // Grid pozisyonunu ve birimlerin listesi d�nd�r
    }

    // Bu GridObject'a bir birim ekler
    public void AddUnit(Unit unit)
    {
        unitList.Add(unit);   // Birimi birim listesine ekle
    }

    // Bu GridObject'tan bir birim ��kar�r
    public void RemoveUnit(Unit unit)
    {
        unitList.Remove(unit);   // Birimi birim listesinden ��kar
    }

    // Bu GridObject �zerinde bulunan t�m birimleri d�ner
    public List<Unit> GetUnitList()
    {
        return unitList;   // Birim listesine eri�im
    }

    // Bu GridObject �zerinde herhangi bir birimin olup olmad���n� kontrol eder
    public bool HasAnyUnit()
    {
        return unitList.Count > 0;   // Birim listesi bo� de�ilse true d�ner
    }

    // Bu GridObject �zerinde bulunan ilk birimi d�ner
    public Unit GetUnit()
    {
        if (HasAnyUnit())   // E�er birim varsa
        {
            return unitList[0];   // �lk birimi d�ner
        }
        else
        {
            return null;   // E�er birim yoksa null d�ner
        }
    }

    // Bu GridObject �zerinde bulunan etkile�imi d�ner
    public IInteractable GetInteractable()
    {
        return interactable;   // Etkile�im nesnesini d�ner
    }

    // Bu GridObject �zerinde bir etkile�im nesnesi ayarlar
    public void SetInteractable(IInteractable interactable)
    {
        this.interactable = interactable;   // Etkile�im nesnesini ayarla
    }

    // Bu GridObject �zerindeki etkile�im nesmesini temizler
    public void ClearInteractable()
    {
        this.interactable = null;   // Etkile�im nesnesini null yap
    }
}
