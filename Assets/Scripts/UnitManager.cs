using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    // Birimleri y�neten sistem

    public static UnitManager Instance { get; private set; }   // Singleton �rne�i

    private List<Unit> unitList;   // Oyun d�nyas�nda bulunan t�m birimler
    private List<Unit> friendlyUnitList;   // Dost birimler listesi
    private List<Unit> enemyUnitList;   // D��man birimler listesi

    private void Awake()
    {
        // Singleton kontrol�: E�er daha �nce bir �rnek varsa mevcut �rne�i sil
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
        // Birimlerin do�ma ve �lme olaylar�na abone ol
        Unit.OnAnyUnitSpawned += Unit_OnAnyUnitSpawned;
        Unit.OnAnyUnitDead += Unit_OnAnyUnitDead;
    }

    // Birim do�ma olay� ger�ekle�ti�inde
    private void Unit_OnAnyUnitSpawned(object sender, EventArgs e)
    {
        Unit unit = sender as Unit;

        unitList.Add(unit);   // T�m birimlerin listesine ekle

        if (unit.IsEnemy())   // D��man birim mi?
        {
            enemyUnitList.Add(unit);   // D��man birimleri listeye ekle
        }
        else
        {
            friendlyUnitList.Add(unit);   // Dost birimleri listeye ekle
        }
    }

    // Birim �lme olay� ger�ekle�ti�inde
    private void Unit_OnAnyUnitDead(object sender, EventArgs e)
    {
        Unit unit = sender as Unit;

        unitList.Remove(unit);   // Birim listeden ��kar

        if (unit.IsEnemy())   // D��man birim mi?
        {
            enemyUnitList.Remove(unit);   // D��man birimleri listeden ��kar
        }
        else
        {
            friendlyUnitList.Remove(unit);   // Dost birimleri listeden ��kar
        }
    }

    public List<Unit> GetUnitList()
    {
        return unitList;   // Oyun d�nyas�ndaki t�m birimleri d�nd�r
    }

    public List<Unit> GetFriendlyUnitList()
    {
        return friendlyUnitList;   // Dost birimleri d�nd�r
    }

    public List<Unit> GetEnemyUnitList()
    {
        return enemyUnitList;   // D��man birimleri d�nd�r
    }
}
