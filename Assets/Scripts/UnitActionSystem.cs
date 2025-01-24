using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitActionSystem : MonoBehaviour
{
    // Birim eylemlerini yöneten sistem

    public static UnitActionSystem Instance { get; private set; }   // Singleton örneði

    public event EventHandler OnSelectedUnitChanged;   // Seçilen birim deðiþtiðinde tetiklenen olay
    public event EventHandler OnSelectedActionChanged;   // Seçilen eylem deðiþtiðinde tetiklenen olay
    public event EventHandler<bool> OnBusyChanged;   // Eylem sýrasýnda meþgul durum deðiþtiðinde tetiklenen olay
    public event EventHandler OnActionStarted;   // Eylem baþladýðýnda tetiklenen olay

    [SerializeField] private Unit selectedUnit;   // Seçilen birim
    [SerializeField] private LayerMask unitLayerMask;   // Birimlerin bulunduðu zemin maskesi

    private BaseAction selectedAction;   // Seçilen eylem
    private bool isBusy;   // Birim eylem sýrasýnda meþgul mü?

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
        SetSelectedUnit(selectedUnit);   // Ýlk seçilen birimi ayarla
    }

    private void Update()
    {
        if (isBusy) return;

        if (!TurnSystem.Instance.IsPlayerTurn()) return;   // Oyuncunun sýrada olmadýðýný kontrol et

        if (EventSystem.current.IsPointerOverGameObject()) return;   // UI üzerinde etkileþim varsa döngüden çýk

        if (TryHandleUnitSelection()) return;   // Birim seçimi kontrol et

        HandleSelectedAction();   // Seçilen eylemi iþleme
    }

    private void HandleSelectedAction()
    {
        if (InputManager.Instance.IsMouseButtonDownThisFrame())   // Mouse sol týklandýysa
        {
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());

            if (!selectedAction.IsValidActionGridPosition(mouseGridPosition)) return;   // Eylem geçerli alan mý?

            if (!selectedUnit.TrySpendActionPointsToTakeAction(selectedAction)) return;   // Hareket puaný yetersizse döngüden çýk

            SetBusy();
            selectedAction.TakeAction(mouseGridPosition, ClearBusy);   // Eylemi gerçekleþtirilmesini saðlar ve iþaretlenmiþ eylemi sonlandýrýr

            OnActionStarted?.Invoke(this, EventArgs.Empty);   // Eylem baþlatýldýðýnda tetiklenir
        }
    }

    private void SetBusy()
    {
        isBusy = true;

        OnBusyChanged?.Invoke(this, isBusy);   // Meþgul durumu güncellenir
    }

    private void ClearBusy()
    {
        isBusy = false;

        OnBusyChanged?.Invoke(this, isBusy);   // Meþgul durumu sona erdiðinde tetiklenir
    }

    private bool TryHandleUnitSelection()
    {
        if (InputManager.Instance.IsMouseButtonDownThisFrame())   // Mouse sol týklandýysa
        {
            Ray ray = Camera.main.ScreenPointToRay(InputManager.Instance.GetMouseScreenPosition());
            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, unitLayerMask))
            {
                if (raycastHit.transform.TryGetComponent<Unit>(out Unit unit))
                {
                    if (unit == selectedUnit)
                    {
                        return false;   // Ayný birimi tekrar seçmek
                    }

                    if (unit.IsEnemy())
                    {
                        return false;   // Düþman birim seçilemez
                    }

                    SetSelectedUnit(unit);   // Yeni birimi seç
                    return true;
                }
            }
        }

        return false;   // Birim seçilmedi
    }

    private void SetSelectedUnit(Unit unit)
    {
        selectedUnit = unit;

        SetSelectedAction(unit.GetAction<MoveAction>());   // Seçilen birimin hareket eylemini ayarla

        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);   // Seçilen birim deðiþtiðinde tetiklenir
    }

    public void SetSelectedAction(BaseAction baseAction)
    {
        selectedAction = baseAction;

        OnSelectedActionChanged?.Invoke(this, EventArgs.Empty);   // Seçilen eylem deðiþtiðinde tetiklenir
    }

    public Unit GetSelectedUnit()
    {
        return selectedUnit;   // Seçili birimi döndürür
    }

    public BaseAction GetSelectedAction()
    {
        return selectedAction;   // Seçili eylemi döndürür
    }

}
