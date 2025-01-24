using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    // T�m eylemleri temel alan s�n�f (Soyut)

    public static event EventHandler OnAnyActionStarted;   // Herhangi bir eylem ba�lad���nda tetiklenir
    public static event EventHandler OnAnyActionCompleted;   // Herhangi bir eylem tamamland���nda tetiklenir

    protected Unit unit;   // Bu eylemi ger�ekle�tiren birim
    protected bool isActive;   // Eylemin aktif olup olmad���
    protected Action onActionComplete;   // Eylem tamamland���nda �a�r�lacak olay

    protected virtual void Awake()
    {
        unit = GetComponent<Unit>();   // Birim nesnesini al
    }

    public abstract string GetActionName();   // Eylemin ad�n� d�nd�r�r

    public abstract void TakeAction(GridPosition gridPosition, Action onActionComplete);   // Eylemi al�r

    public virtual bool IsValidActionGridPosition(GridPosition gridPosition)   // Ge�erli eylem konumunu kontrol eder
    {
        List<GridPosition> validGridPositionList = GetValidActionGridPositionList();   // Ge�erli eylem konumlar�n� al
        return validGridPositionList.Contains(gridPosition);
    }

    public abstract List<GridPosition> GetValidActionGridPositionList();   // Ge�erli eylem konumlar�n� d�nd�r�r

    public virtual int GetActionPointsCost()   // Eylem puan maliyetini d�nd�r�r
    {
        return 1;
    }

    protected void ActionStart(Action onActionComplete)   // Eylem ba�latma i�lemi
    {
        isActive = true;   // Eylemi aktif et
        this.onActionComplete = onActionComplete;

        OnAnyActionStarted?.Invoke(this, EventArgs.Empty);   // Eylem ba�lad���nda tetikleyici
    }

    protected void ActionComplete()   // Eylemi tamamlamaya y�nelik i�lem
    {
        isActive = false;   // Eylemi aktif olmaktan ��kar
        onActionComplete();   // Tamamland�ktan sonra belirtilen i�lem �a�r�l�r

        OnAnyActionCompleted?.Invoke(this, EventArgs.Empty);   // Eylem tamamland���nda tetikleyici
    }

    public Unit GetUnit()
    {
        return unit;   // Bu eylemi ger�ekle�tiren birimi d�nd�r�r
    }

    public EnemyAIAction GetBestEnemyAIAction()   // En iyi d��man AI eylemini hesaplar
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
            // En y�ksek puana sahip eylemi d�nd�r
            enemyAIActionList.Sort((EnemyAIAction a, EnemyAIAction b) => b.actionValue - a.actionValue);
            return enemyAIActionList[0];
        }
        else
        {
            // Hi�bir olas� d��man AI eylemi yoksa
            return null;
        }
    }

    public abstract EnemyAIAction GetEnemyAIAction(GridPosition gridPosition);   // D��man AI eylemlerini d�nd�r�r
}
