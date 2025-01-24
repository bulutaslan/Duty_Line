using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    // Birimleri yöneten sistem

    public static UnitManager Instance { get; private set; }   // Singleton örneði

    private List<Unit> unitList;   // Oyun dünyasýnda bulunan tüm birimler
    private List<Unit> friendlyUnitList;   // Dost birimler listesi
    private List<Unit> enemyUnitList;   // Düþman birimler listesi

    private void Awake()
    {
        // Singleton kontrolü: Eðer daha önce bir örnek varsa mevcut örneði sil
        if (Instance != null)
        {
            Debug.LogError("There's more than one UnitManager! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;

        unitList = new List<Unit>();
        friendlyUnitList = new List<Unit>();
        enemyUnitList = new List<Unit>();
    }

    private void Start()
    {
        // Birimlerin doðma ve ölme olaylarýna abone ol
        Unit.OnAnyUnitSpawned += Unit_OnAnyUnitSpawned;
        Unit.OnAnyUnitDead += Unit_OnAnyUnitDead;
    }

    // Birim doðma olayý gerçekleþtiðinde
    private void Unit_OnAnyUnitSpawned(object sender, EventArgs e)
    {
        Unit unit = sender as Unit;

        unitList.Add(unit);   // Tüm birimlerin listesine ekle

        if (unit.IsEnemy())   // Düþman birim mi?
        {
            enemyUnitList.Add(unit);   // Düþman birimleri listeye ekle
        }
        else
        {
            friendlyUnitList.Add(unit);   // Dost birimleri listeye ekle
        }
    }

    // Birim ölme olayý gerçekleþtiðinde
    private void Unit_OnAnyUnitDead(object sender, EventArgs e)
    {
        Unit unit = sender as Unit;

        unitList.Remove(unit);   // Birim listeden çýkar

        if (unit.IsEnemy())   // Düþman birim mi?
        {
            enemyUnitList.Remove(unit);   // Düþman birimleri listeden çýkar
        }
        else
        {
            friendlyUnitList.Remove(unit);   // Dost birimleri listeden çýkar
        }
    }

    public List<Unit> GetUnitList()
    {
        return unitList;   // Oyun dünyasýndaki tüm birimleri döndür
    }

    public List<Unit> GetFriendlyUnitList()
    {
        return friendlyUnitList;   // Dost birimleri döndür
    }

    public List<Unit> GetEnemyUnitList()
    {
        return enemyUnitList;   // Düþman birimleri döndür
    }
}
