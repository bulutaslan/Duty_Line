using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitActionSystem : MonoBehaviour
{
    // Birim eylemlerini y�neten sistem

    public static UnitActionSystem Instance { get; private set; }   // Singleton �rne�i

    public event EventHandler OnSelectedUnitChanged;   // Se�ilen birim de�i�ti�inde tetiklenen olay
    public event EventHandler OnSelectedActionChanged;   // Se�ilen eylem de�i�ti�inde tetiklenen olay
    public event EventHandler<bool> OnBusyChanged;   // Eylem s�ras�nda me�gul durum de�i�ti�inde tetiklenen olay
    public event EventHandler OnActionStarted;   // Eylem ba�lad���nda tetiklenen olay

    [SerializeField] private Unit selectedUnit;   // Se�ilen birim
    [SerializeField] private LayerMask unitLayerMask;   // Birimlerin bulundu�u zemin maskesi

    private BaseAction selectedAction;   // Se�ilen eylem
    private bool isBusy;   // Birim eylem s�ras�nda me�gul m�?

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one UnitActionSystem! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        SetSelectedUnit(selectedUnit);   // �lk se�ilen birimi ayarla
    }

    private void Update()
    {
        if (isBusy) return;

        if (!TurnSystem.Instance.IsPlayerTurn()) return;   // Oyuncunun s�rada olmad���n� kontrol et

        if (EventSystem.current.IsPointerOverGameObject()) return;   // UI �zerinde etkile�im varsa d�ng�den ��k

        if (TryHandleUnitSelection()) return;   // Birim se�imi kontrol et

        HandleSelectedAction();   // Se�ilen eylemi i�leme
    }

    private void HandleSelectedAction()
    {
        if (InputManager.Instance.IsMouseButtonDownThisFrame())   // Mouse sol t�kland�ysa
        {
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());

            if (!selectedAction.IsValidActionGridPosition(mouseGridPosition)) return;   // Eylem ge�erli alan m�?

            if (!selectedUnit.TrySpendActionPointsToTakeAction(selectedAction)) return;   // Hareket puan� yetersizse d�ng�den ��k

            SetBusy();
            selectedAction.TakeAction(mouseGridPosition, ClearBusy);   // Eylemi ger�ekle�tirilmesini sa�lar ve i�aretlenmi� eylemi sonland�r�r

            OnActionStarted?.Invoke(this, EventArgs.Empty);   // Eylem ba�lat�ld���nda tetiklenir
        }
    }

    private void SetBusy()
    {
        isBusy = true;

        OnBusyChanged?.Invoke(this, isBusy);   // Me�gul durumu g�ncellenir
    }

    private void ClearBusy()
    {
        isBusy = false;

        OnBusyChanged?.Invoke(this, isBusy);   // Me�gul durumu sona erdi�inde tetiklenir
    }

    private bool TryHandleUnitSelection()
    {
        if (InputManager.Instance.IsMouseButtonDownThisFrame())   // Mouse sol t�kland�ysa
        {
            Ray ray = Camera.main.ScreenPointToRay(InputManager.Instance.GetMouseScreenPosition());
            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, unitLayerMask))
            {
                if (raycastHit.transform.TryGetComponent<Unit>(out Unit unit))
                {
                    if (unit == selectedUnit)
                    {
                        return false;   // Ayn� birimi tekrar se�mek
                    }

                    if (unit.IsEnemy())
                    {
                        return false;   // D��man birim se�ilemez
                    }

                    SetSelectedUnit(unit);   // Yeni birimi se�
                    return true;
                }
            }
        }

        return false;   // Birim se�ilmedi
    }

    private void SetSelectedUnit(Unit unit)
    {
        selectedUnit = unit;

        SetSelectedAction(unit.GetAction<MoveAction>());   // Se�ilen birimin hareket eylemini ayarla

        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);   // Se�ilen birim de�i�ti�inde tetiklenir
    }

    public void SetSelectedAction(BaseAction baseAction)
    {
        selectedAction = baseAction;

        OnSelectedActionChanged?.Invoke(this, EventArgs.Empty);   // Se�ilen eylem de�i�ti�inde tetiklenir
    }

    public Unit GetSelectedUnit()
    {
        return selectedUnit;   // Se�ili birimi d�nd�r�r
    }

    public BaseAction GetSelectedAction()
    {
        return selectedAction;   // Se�ili eylemi d�nd�r�r
    }

}
