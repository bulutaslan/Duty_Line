using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAction : BaseAction
{
    // K�l�� eylemi

    public static event EventHandler OnAnySwordHit;   // Herhangi bir k�l�� darbesi olay�n� tetikler

    public event EventHandler OnSwordActionStarted;   // Belirli bir k�l�� eylemini ba�lat�r
    public event EventHandler OnSwordActionCompleted;   // Belirli bir k�l�� eylemini tamamlar

    private enum State
    {
        SwingingSwordBeforeHit,   // Vuru�tan �nce k�l�c� savurma
        SwingingSwordAfterHit,   // Vuru�tan sonra k�l�c� savurma
    }

    private int maxSwordDistance = 1;   // Maksimum k�l�� mesafesi
    private State state;   // Eylemin durumu
    private float stateTimer;   // Eylem durumu zamanlay�c�
    private Unit targetUnit;   // Hedef birim

    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        stateTimer -= Time.deltaTime;   // Zamanlay�c� g�ncellemesi

        switch (state)
        {
            case State.SwingingSwordBeforeHit:
                Vector3 aimDir = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
                float rotateSpeed = 10f;
                transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * rotateSpeed);
                break;
            case State.SwingingSwordAfterHit:
                break;
        }

        if (stateTimer <= 0f)
        {
            NextState();   // Sonraki duruma ge�
        }
    }

    private void NextState()
    {
        switch (state)
        {
            case State.SwingingSwordBeforeHit:
                state = State.SwingingSwordAfterHit;
                float afterHitStateTime = 0.5f;
                stateTimer = afterHitStateTime;
                targetUnit.Damage(100);   // Hasar ver
                OnAnySwordHit?.Invoke(this, EventArgs.Empty);   // Herhangi bir k�l�� darbesi
                break;
            case State.SwingingSwordAfterHit:
                OnSwordActionCompleted?.Invoke(this, EventArgs.Empty);   // Eylem tamamland�
                ActionComplete();
                break;
        }
    }

    public override string GetActionName()
    {
        return "Sword";   // Eylem ad�
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 200,   // AI eylem de�eri
        };
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxSwordDistance; x <= maxSwordDistance; x++)
        {
            for (int z = -maxSwordDistance; z <= maxSwordDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;   // Ge�ersiz grid konumu
                }

                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    continue;   // Grid pozisyonu bo�, herhangi bir birim yok
                }

                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);

                if (targetUnit.IsEnemy() == unit.IsEnemy())
                {
                    continue;   // Ayn� 'tak�m' birimleri
                }

                validGridPositionList.Add(testGridPosition);   // Ge�erli k�l�� darbesi pozisyonu
            }
        }

        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        state = State.SwingingSwordBeforeHit;
        float beforeHitStateTime = 0.7f;
        stateTimer = beforeHitStateTime;

        OnSwordActionStarted?.Invoke(this, EventArgs.Empty);   // K�l�� eylemi ba�lad�

        ActionStart(onActionComplete);
    }

    public int GetMaxSwordDistance()
    {
        return maxSwordDistance;   // Maksimum k�l�� mesafesini d�nd�r
    }
}
