using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    // Tüm eylemleri temel alan sýnýf (Soyut)

    public static event EventHandler OnAnyActionStarted;   // Herhangi bir eylem baþladýðýnda tetiklenir
    public static event EventHandler OnAnyActionCompleted;   // Herhangi bir eylem tamamlandýðýnda tetiklenir

    protected Unit unit;   // Bu eylemi gerçekleþtiren birim
    protected bool isActive;   // Eylemin aktif olup olmadýðý
    protected Action onActionComplete;   // Eylem tamamlandýðýnda çaðrýlacak olay

    protected virtual void Awake()
    {
        unit = GetComponent<Unit>();   // Birim nesnesini al
    }

    public abstract string GetActionName();   // Eylemin adýný döndürür

    public abstract void TakeAction(GridPosition gridPosition, Action onActionComplete);   // Eylemi alýr

    public virtual bool IsValidActionGridPosition(GridPosition gridPosition)   // Geçerli eylem konumunu kontrol eder
    {
        List<GridPosition> validGridPositionList = GetValidActionGridPositionList();   // Geçerli eylem konumlarýný al
        return validGridPositionList.Contains(gridPosition);
    }

    public abstract List<GridPosition> GetValidActionGridPositionList();   // Geçerli eylem konumlarýný döndürür

    public virtual int GetActionPointsCost()   // Eylem puan maliyetini döndürür
    {
        return 1;
    }

    protected void ActionStart(Action onActionComplete)   // Eylem baþlatma iþlemi
    {
        isActive = true;   // Eylemi aktif et
        this.onActionComplete = onActionComplete;

        OnAnyActionStarted?.Invoke(this, EventArgs.Empty);   // Eylem baþladýðýnda tetikleyici
    }

    protected void ActionComplete()   // Eylemi tamamlamaya yönelik iþlem
    {
        isActive = false;   // Eylemi aktif olmaktan çýkar
        onActionComplete();   // Tamamlandýktan sonra belirtilen iþlem çaðrýlýr

        OnAnyActionCompleted?.Invoke(this, EventArgs.Empty);   // Eylem tamamlandýðýnda tetikleyici
    }

    public Unit GetUnit()
    {
        return unit;   // Bu eylemi gerçekleþtiren birimi döndürür
    }

    public EnemyAIAction GetBestEnemyAIAction()   // En iyi düþman AI eylemini hesaplar
    {
        List<EnemyAIAction> enemyAIActionList = new List<EnemyAIAction>();

        List<GridPosition> validActionGridPositionList = GetValidActionGridPositionList();

        foreach (GridPosition gridPosition in validActionGridPositionList)
        {
            EnemyAIAction enemyAIAction = GetEnemyAIAction(gridPosition);
            enemyAIActionList.Add(enemyAIAction);
        }

        if (enemyAIActionList.Count > 0)
        {
            // En yüksek puana sahip eylemi döndür
            enemyAIActionList.Sort((EnemyAIAction a, EnemyAIAction b) => b.actionValue - a.actionValue);
            return enemyAIActionList[0];
        }
        else
        {
            // Hiçbir olasý düþman AI eylemi yoksa
            return null;
        }
    }

    public abstract EnemyAIAction GetEnemyAIAction(GridPosition gridPosition);   // Düþman AI eylemlerini döndürür
}
