using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAction : BaseAction
{
    // Kýlýç eylemi

    public static event EventHandler OnAnySwordHit;   // Herhangi bir kýlýç darbesi olayýný tetikler

    public event EventHandler OnSwordActionStarted;   // Belirli bir kýlýç eylemini baþlatýr
    public event EventHandler OnSwordActionCompleted;   // Belirli bir kýlýç eylemini tamamlar

    private enum State
    {
        SwingingSwordBeforeHit,   // Vuruþtan önce kýlýcý savurma
        SwingingSwordAfterHit,   // Vuruþtan sonra kýlýcý savurma
    }

    private int maxSwordDistance = 1;   // Maksimum kýlýç mesafesi
    private State state;   // Eylemin durumu
    private float stateTimer;   // Eylem durumu zamanlayýcý
    private Unit targetUnit;   // Hedef birim

    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        stateTimer -= Time.deltaTime;   // Zamanlayýcý güncellemesi

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
            NextState();   // Sonraki duruma geç
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
                OnAnySwordHit?.Invoke(this, EventArgs.Empty);   // Herhangi bir kýlýç darbesi
                break;
            case State.SwingingSwordAfterHit:
                OnSwordActionCompleted?.Invoke(this, EventArgs.Empty);   // Eylem tamamlandý
                ActionComplete();
                break;
        }
    }

    public override string GetActionName()
    {
        return "Sword";   // Eylem adý
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 200,   // AI eylem deðeri
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
                    continue;   // Geçersiz grid konumu
                }

                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    continue;   // Grid pozisyonu boþ, herhangi bir birim yok
                }

                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);

                if (targetUnit.IsEnemy() == unit.IsEnemy())
                {
                    continue;   // Ayný 'takým' birimleri
                }

                validGridPositionList.Add(testGridPosition);   // Geçerli kýlýç darbesi pozisyonu
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

        OnSwordActionStarted?.Invoke(this, EventArgs.Empty);   // Kýlýç eylemi baþladý

        ActionStart(onActionComplete);
    }

    public int GetMaxSwordDistance()
    {
        return maxSwordDistance;   // Maksimum kýlýç mesafesini döndür
    }
}
