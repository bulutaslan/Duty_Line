using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAction : BaseAction
{
    // Ate� etme eylemi

    public static event EventHandler<OnShootEventArgs> OnAnyShoot;   // Herhangi bir ate� etme olay�n� tetikler

    public event EventHandler<OnShootEventArgs> OnShoot;   // Belirli bir ate� etme olay�n� tetikler

    public class OnShootEventArgs : EventArgs
    {
        public Unit targetUnit;   // Ate� edilen hedef birim
        public Unit shootingUnit;   // Ate� eden birim
    }

    private enum State
    {
        Aiming,   // Ni�an alma durumu
        Shooting,   // Ate� etme durumu
        Cooloff,   // So�uma durumu
    }

    [SerializeField] private LayerMask obstaclesLayerMask;   // Engel katmanlar�n�n maskesi

    private State state;   // Eylemin durumu
    private int maxShootDistance = 7;   // Maksimum at�� mesafesi
    private float stateTimer;   // Eylem durumu zamanlay�c�
    private Unit targetUnit;   // Hedef birim
    private bool canShootBullet;   // Mermi atma yetene�i

    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        stateTimer -= Time.deltaTime;   // Zamanlay�c� g�ncellemesi

        switch (state)
        {
            case State.Aiming:
                Vector3 aimDir = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
                float rotateSpeed = 10f;
                transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * rotateSpeed);
                break;
            case State.Shooting:
                if (canShootBullet)
                {
                    Shoot();   // Ate� et
                    canShootBullet = false;   // Mermi at�ld�
                }
                break;
            case State.Cooloff:
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
            case State.Aiming:
                state = State.Shooting;
                float shootingStateTime = 0.1f;
                stateTimer = shootingStateTime;
                break;
            case State.Shooting:
                state = State.Cooloff;
                float coolOffStateTime = 0.5f;
                stateTimer = coolOffStateTime;
                break;
            case State.Cooloff:
                ActionComplete();   // Eylemi tamamla
                break;
        }
    }

    private void Shoot()
    {
        OnAnyShoot?.Invoke(this, new OnShootEventArgs
        {
            targetUnit = targetUnit,
            shootingUnit = unit
        });

        OnShoot?.Invoke(this, new OnShootEventArgs
        {
            targetUnit = targetUnit,
            shootingUnit = unit
        });

        targetUnit.Damage(40);   // Hedef birimi hasar ver
    }

    public override string GetActionName()
    {
        return "Shoot";   // Eylem ad�
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();
        return GetValidActionGridPositionList(unitGridPosition);
    }

    public List<GridPosition> GetValidActionGridPositionList(GridPosition unitGridPosition)
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        for (int x = -maxShootDistance; x <= maxShootDistance; x++)
        {
            for (int z = -maxShootDistance; z <= maxShootDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;   // Ge�ersiz grid konumu
                }

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > maxShootDistance)
                {
                    continue;   // Maksimum at�� mesafesini a�an konumlar
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

                Vector3 unitWorldPosition = LevelGrid.Instance.GetWorldPosition(unitGridPosition);
                Vector3 shootDir = (targetUnit.GetWorldPosition() - unitWorldPosition).normalized;

                float unitShoulderHeight = 1.7f;
                if (Physics.Raycast(
                        unitWorldPosition + Vector3.up * unitShoulderHeight,
                        shootDir,
                        Vector3.Distance(unitWorldPosition, targetUnit.GetWorldPosition()),
                        obstaclesLayerMask))
                {
                    continue;   // Engel oldu�u i�in at�� m�mk�n de�il
                }

                validGridPositionList.Add(testGridPosition);   // Ge�erli at�� pozisyonu
            }
        }

        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        state = State.Aiming;
        float aimingStateTime = 1f;
        stateTimer = aimingStateTime;

        canShootBullet = true;

        ActionStart(onActionComplete);
    }

    public Unit GetTargetUnit()
    {
        return targetUnit;   // Hedef birimi d�nd�r
    }

    public int GetMaxShootDistance()
    {
        return maxShootDistance;   // Maksimum at�� mesafesini d�nd�r
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 100 + Mathf.RoundToInt((1 - targetUnit.GetHealthNormalized()) * 100f),
        };
    }

    public int GetTargetCountAtPosition(GridPosition gridPosition)
    {
        return GetValidActionGridPositionList(gridPosition).Count;   // Bu pozisyondaki hedef birim say�s�n� d�nd�r
    }
}
