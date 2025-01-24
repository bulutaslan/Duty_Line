using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    // Birim s�n�f�, oyun d�nyas�nda hareket eden ve etkile�imde bulunan objeyi temsil eder.

    private const int ACTION_POINTS_MAX = 3;   // Bir birimin maksimum hareket puan�

    public static event EventHandler OnAnyActionPointsChanged;   // Hareket puan� de�i�ti�inde tetiklenen olay
    public static event EventHandler OnAnyUnitSpawned;           // Birim do�du�unda tetiklenen olay
    public static event EventHandler OnAnyUnitDead;              // Birim �ld���nde tetiklenen olay

    [SerializeField] private bool isEnemy;   // Bu birimin d��man olup olmad���n� belirler

    private GridPosition gridPosition;        // Birimin konum bilgisi
    private HealthSystem healthSystem;        // Sa�l�k sistemi bilgileri
    private BaseAction[] baseActionArray;     // Birimin ger�ekle�tirebilece�i temel eylemler
    private int actionPoints = ACTION_POINTS_MAX;   // Kullan�labilir hareket puan�

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();   // Sa�l�k sistemi bile�enini al
        baseActionArray = GetComponents<BaseAction>();   // Temel eylemleri al
    }

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);   // Konumu grid'e g�re al
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);   // Grid �zerinde bu birimi konumland�r

        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;   // S�ra de�i�ti�inde tetiklenen olay

        healthSystem.OnDead += HealthSystem_OnDead;   // Sa�l�k sistemi �lme olay�

        OnAnyUnitSpawned?.Invoke(this, EventArgs.Empty);   // Birim do�du�unda tetiklenir
    }

    private void Update()
    {
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);   // Birimin g�ncel konumu
        if (newGridPosition != gridPosition)
        {
            // Birim konumunu de�i�tirdi
            GridPosition oldGridPosition = gridPosition;
            gridPosition = newGridPosition;

            LevelGrid.Instance.UnitMovedGridPosition(this, oldGridPosition, newGridPosition);   // Grid konumunu g�ncelle
        }
    }

    public T GetAction<T>() where T : BaseAction
    {
        foreach (BaseAction baseAction in baseActionArray)
        {
            if (baseAction is T)
            {
                return (T)baseAction;
            }
        }
        return null;   // Belirtilen t�rde bir eylem bulunamad�
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;   // Bu birimin grid konumunu d�nd�r�r
    }

    public Vector3 GetWorldPosition()
    {
        return transform.position;   // Bu birimin d�nya �zerindeki konumunu d�nd�r�r
    }

    public BaseAction[] GetBaseActionArray()
    {
        return baseActionArray;   // Bu birimin ger�ekle�tirebilece�i temel eylemleri d�nd�r�r
    }

    public bool TrySpendActionPointsToTakeAction(BaseAction baseAction)
    {
        if (CanSpendActionPointsToTakeAction(baseAction))
        {
            SpendActionPoints(baseAction.GetActionPointsCost());
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CanSpendActionPointsToTakeAction(BaseAction baseAction)
    {
        if (actionPoints >= baseAction.GetActionPointsCost())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void SpendActionPoints(int amount)
    {
        actionPoints -= amount;   // Hareket puanlar�ndan belirli bir miktar azalt�l�r

        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);   // Hareket puan� de�i�ti�inde tetiklenir
    }

    public int GetActionPoints()
    {
        return actionPoints;   // Kullan�labilir hareket puan�n� d�nd�r�r
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        if ((IsEnemy() && !TurnSystem.Instance.IsPlayerTurn()) ||   // D��man s�rada de�ilse
            (!IsEnemy() && TurnSystem.Instance.IsPlayerTurn()))    // D��man s�radaysa
        {
            actionPoints = ACTION_POINTS_MAX;   // Hareket puanlar� s�f�rlan�r

            OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public bool IsEnemy()
    {
        return isEnemy;   // Bu birimin d��man olup olmad���n� d�nd�r�r
    }

    public void Damage(int damageAmount)
    {
        healthSystem.Damage(damageAmount);   // Birime hasar uygular
    }

    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        LevelGrid.Instance.RemoveUnitAtGridPosition(gridPosition, this);   // Birimi grid'den kald�r

        Destroy(gameObject);   // Bu objeyi yok et

        OnAnyUnitDead?.Invoke(this, EventArgs.Empty);   // Birim �ld���nde tetiklenir
    }

    public float GetHealthNormalized()
    {
        return healthSystem.GetHealthNormalized();   // Birimin sa�l�k durumunu d�nd�r�r
    }

}
