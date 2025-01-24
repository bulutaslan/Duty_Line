using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    // Birim sýnýfý, oyun dünyasýnda hareket eden ve etkileþimde bulunan objeyi temsil eder.

    private const int ACTION_POINTS_MAX = 3;   // Bir birimin maksimum hareket puaný

    public static event EventHandler OnAnyActionPointsChanged;   // Hareket puaný deðiþtiðinde tetiklenen olay
    public static event EventHandler OnAnyUnitSpawned;           // Birim doðduðunda tetiklenen olay
    public static event EventHandler OnAnyUnitDead;              // Birim öldüðünde tetiklenen olay

    [SerializeField] private bool isEnemy;   // Bu birimin düþman olup olmadýðýný belirler

    private GridPosition gridPosition;        // Birimin konum bilgisi
    private HealthSystem healthSystem;        // Saðlýk sistemi bilgileri
    private BaseAction[] baseActionArray;     // Birimin gerçekleþtirebileceði temel eylemler
    private int actionPoints = ACTION_POINTS_MAX;   // Kullanýlabilir hareket puaný

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();   // Saðlýk sistemi bileþenini al
        baseActionArray = GetComponents<BaseAction>();   // Temel eylemleri al
    }

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);   // Konumu grid'e göre al
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);   // Grid üzerinde bu birimi konumlandýr

        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;   // Sýra deðiþtiðinde tetiklenen olay

        healthSystem.OnDead += HealthSystem_OnDead;   // Saðlýk sistemi ölme olayý

        OnAnyUnitSpawned?.Invoke(this, EventArgs.Empty);   // Birim doðduðunda tetiklenir
    }

    private void Update()
    {
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);   // Birimin güncel konumu
        if (newGridPosition != gridPosition)
        {
            // Birim konumunu deðiþtirdi
            GridPosition oldGridPosition = gridPosition;
            gridPosition = newGridPosition;

            LevelGrid.Instance.UnitMovedGridPosition(this, oldGridPosition, newGridPosition);   // Grid konumunu güncelle
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
        return null;   // Belirtilen türde bir eylem bulunamadý
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;   // Bu birimin grid konumunu döndürür
    }

    public Vector3 GetWorldPosition()
    {
        return transform.position;   // Bu birimin dünya üzerindeki konumunu döndürür
    }

    public BaseAction[] GetBaseActionArray()
    {
        return baseActionArray;   // Bu birimin gerçekleþtirebileceði temel eylemleri döndürür
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
        actionPoints -= amount;   // Hareket puanlarýndan belirli bir miktar azaltýlýr

        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);   // Hareket puaný deðiþtiðinde tetiklenir
    }

    public int GetActionPoints()
    {
        return actionPoints;   // Kullanýlabilir hareket puanýný döndürür
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        if ((IsEnemy() && !TurnSystem.Instance.IsPlayerTurn()) ||   // Düþman sýrada deðilse
            (!IsEnemy() && TurnSystem.Instance.IsPlayerTurn()))    // Düþman sýradaysa
        {
            actionPoints = ACTION_POINTS_MAX;   // Hareket puanlarý sýfýrlanýr

            OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public bool IsEnemy()
    {
        return isEnemy;   // Bu birimin düþman olup olmadýðýný döndürür
    }

    public void Damage(int damageAmount)
    {
        healthSystem.Damage(damageAmount);   // Birime hasar uygular
    }

    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        LevelGrid.Instance.RemoveUnitAtGridPosition(gridPosition, this);   // Birimi grid'den kaldýr

        Destroy(gameObject);   // Bu objeyi yok et

        OnAnyUnitDead?.Invoke(this, EventArgs.Empty);   // Birim öldüðünde tetiklenir
    }

    public float GetHealthNormalized()
    {
        return healthSystem.GetHealthNormalized();   // Birimin saðlýk durumunu döndürür
    }

}
