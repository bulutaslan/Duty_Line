using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject
{
    // Bu sýnýf, bir Grid sistemi üzerinde bir hücrenin sahip olduðu nesne ve etkileþimleri tanýmlar

    private GridSystem<GridObject> gridSystem;   // Grid sistemi nesnesi
    private GridPosition gridPosition;   // Bu GridObject'ýn pozisyonu
    private List<Unit> unitList;   // Bu GridObject üzerinde bulunan birimlerin listesi
    private IInteractable interactable;   // Bu GridObject üzerinde bulunan etkileþim nesnesi

    // Constructor: GridObject, Grid sistemi ve bu grid pozisyonunu alýr
    public GridObject(GridSystem<GridObject> gridSystem, GridPosition gridPosition)
    {
        this.gridSystem = gridSystem;   // Grid sistemi nesnesini ata
        this.gridPosition = gridPosition;   // Grid pozisyonunu ata
        unitList = new List<Unit>();   // Birim listesi baþlat
    }

    // Bu GridObject'ýn ToString() metodu, Grid pozisyonunu ve bu pozisyondaki birimleri döndürür
    public override string ToString()
    {
        string unitString = "";
        foreach (Unit unit in unitList)
        {
            unitString += unit + "\n";   // Birimlerin metinsel temsillerini birleþtir
        }

        return gridPosition.ToString() + "\n" + unitString;   // Grid pozisyonunu ve birimlerin listesi döndür
    }

    // Bu GridObject'a bir birim ekler
    public void AddUnit(Unit unit)
    {
        unitList.Add(unit);   // Birimi birim listesine ekle
    }

    // Bu GridObject'tan bir birim çýkarýr
    public void RemoveUnit(Unit unit)
    {
        unitList.Remove(unit);   // Birimi birim listesinden çýkar
    }

    // Bu GridObject üzerinde bulunan tüm birimleri döner
    public List<Unit> GetUnitList()
    {
        return unitList;   // Birim listesine eriþim
    }

    // Bu GridObject üzerinde herhangi bir birimin olup olmadýðýný kontrol eder
    public bool HasAnyUnit()
    {
        return unitList.Count > 0;   // Birim listesi boþ deðilse true döner
    }

    // Bu GridObject üzerinde bulunan ilk birimi döner
    public Unit GetUnit()
    {
        if (HasAnyUnit())   // Eðer birim varsa
        {
            return unitList[0];   // Ýlk birimi döner
        }
        else
        {
            return null;   // Eðer birim yoksa null döner
        }
    }

    // Bu GridObject üzerinde bulunan etkileþimi döner
    public IInteractable GetInteractable()
    {
        return interactable;   // Etkileþim nesnesini döner
    }

    // Bu GridObject üzerinde bir etkileþim nesnesi ayarlar
    public void SetInteractable(IInteractable interactable)
    {
        this.interactable = interactable;   // Etkileþim nesnesini ayarla
    }

    // Bu GridObject üzerindeki etkileþim nesmesini temizler
    public void ClearInteractable()
    {
        this.interactable = null;   // Etkileþim nesnesini null yap
    }
}
