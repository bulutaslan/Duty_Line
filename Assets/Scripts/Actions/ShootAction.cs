using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAction : BaseAction
{
    // Ateþ etme eylemi

    public static event EventHandler<OnShootEventArgs> OnAnyShoot;   // Herhangi bir ateþ etme olayýný tetikler

    public event EventHandler<OnShootEventArgs> OnShoot;   // Belirli bir ateþ etme olayýný tetikler

    public class OnShootEventArgs : EventArgs
    {
        public Unit targetUnit;   // Ateþ edilen hedef birim
        public Unit shootingUnit;   // Ateþ eden birim
    }

    private enum State
    {
        Aiming,   // Niþan alma durumu
        Shooting,   // Ateþ etme durumu
        Cooloff,   // Soðuma durumu
    }

    [SerializeField] private LayerMask obstaclesLayerMask;   // Engel katmanlarýnýn maskesi

    private State state;   // Eylemin durumu
    private int maxShootDistance = 7;   // Maksimum atýþ mesafesi
    private float stateTimer;   // Eylem durumu zamanlayýcý
    private Unit targetUnit;   // Hedef birim
    private bool canShootBullet;   // Mermi atma yeteneði

    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        stateTimer -= Time.deltaTime;   // Zamanlayýcý güncellemesi

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
                    Shoot();   // Ateþ et
                    canShootBullet = false;   // Mermi atýldý
                }
                break;
            case State.Cooloff:
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
        return "Shoot";   // Eylem adý
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
                    continue;   // Geçersiz grid konumu
                }

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > maxShootDistance)
                {
                    continue;   // Maksimum atýþ mesafesini aþan konumlar
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

                Vector3 unitWorldPosition = LevelGrid.Instance.GetWorldPosition(unitGridPosition);
                Vector3 shootDir = (targetUnit.GetWorldPosition() - unitWorldPosition).normalized;

                float unitShoulderHeight = 1.7f;
                if (Physics.Raycast(
                        unitWorldPosition + Vector3.up * unitShoulderHeight,
                        shootDir,
                        Vector3.Distance(unitWorldPosition, targetUnit.GetWorldPosition()),
                        obstaclesLayerMask))
                {
                    continue;   // Engel olduðu için atýþ mümkün deðil
                }

                validGridPositionList.Add(testGridPosition);   // Geçerli atýþ pozisyonu
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
        return targetUnit;   // Hedef birimi döndür
    }

    public int GetMaxShootDistance()
    {
        return maxShootDistance;   // Maksimum atýþ mesafesini döndür
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
        return GetValidActionGridPositionList(gridPosition).Count;   // Bu pozisyondaki hedef birim sayýsýný döndür
    }
}
