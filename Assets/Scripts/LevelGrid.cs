using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    // Singleton örneði, LevelGrid sýnýfýnýn sadece bir örneðinin varlýðýný saðlar.
    public static LevelGrid Instance { get; private set; }

    // Oyun alanýnda herhangi bir birimin konum deðiþikliðini izleyen olay.
    public event EventHandler<OnAnyUnitMovedGridPositionEventArgs> OnAnyUnitMovedGridPosition;

    public class OnAnyUnitMovedGridPositionEventArgs : EventArgs
    {
        public Unit unit;                  // Birim nesnesi
        public GridPosition fromGridPosition;  // Birimin baþladýðý hücre konumu
        public GridPosition toGridPosition;    // Birimin gittiði hücre konumu
    }

    [SerializeField] private Transform gridDebugObjectPrefab;  // Aðý, hücreleri görselleþtirmek için kullanýlacak nesne
    [SerializeField] private int width;  // Aðýrlýk geniþliði
    [SerializeField] private int height; // Aðý geniþliði
    [SerializeField] private float cellSize; // Hücre boyutu

    private GridSystem<GridObject> gridSystem; // Aðý iþlemek için GridSystem nesnesi

    // Her oyun nesnesi uyanmadan önce çaðrýlan metod
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("LevelGrid örneði zaten mevcut! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // Grid sistemini oluþturuyoruz ve her hücrede GridObject nesnesini tutuyoruz.
        gridSystem = new GridSystem<GridObject>(width, height, cellSize,
            (GridSystem<GridObject> g, GridPosition gridPosition) => new GridObject(g, gridPosition));
        // gridSystem.CreateDebugObjects(gridDebugObjectPrefab); // Debug nesneleri oluþturulabilir
    }

    private void Start()
    {
        // Aðýrlýk sistemi için baþlangýç ayarlarýný yapýyoruz.
        Pathfinding.Instance.Setup(width, height, cellSize);
    }

    // Hücre konumuna bir birimi ekler.
    public void AddUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.AddUnit(unit);
    }

    // Belirli bir hücre konumunda bulunan tüm birimleri döner.
    public List<Unit> GetUnitListAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetUnitList();
    }

    // Belirli bir hücre konumundan bir birimi kaldýrýr.
    public void RemoveUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.RemoveUnit(unit);
    }

    // Bir birimin hücre konumunu günceller ve olay tetiklenir.
    public void UnitMovedGridPosition(Unit unit, GridPosition fromGridPosition, GridPosition toGridPosition)
    {
        RemoveUnitAtGridPosition(fromGridPosition, unit);  // Önce birimi kaldýr
        AddUnitAtGridPosition(toGridPosition, unit);       // Sonra yeni konumda ekle

        // Konum deðiþikliðini dinleyicilere bildir
        OnAnyUnitMovedGridPosition?.Invoke(this, new OnAnyUnitMovedGridPositionEventArgs
        {
            unit = unit,
            fromGridPosition = fromGridPosition,
            toGridPosition = toGridPosition,
        });
    }

    public GridPosition GetGridPosition(Vector3 worldPosition) => gridSystem.GetGridPosition(worldPosition);  // Dünyadan grid konumunu al
    public Vector3 GetWorldPosition(GridPosition gridPosition) => gridSystem.GetWorldPosition(gridPosition);      // Grid konumundan dünyaya pozisyon al
    public bool IsValidGridPosition(GridPosition gridPosition) => gridSystem.IsValidGridPosition(gridPosition); // Geçerli grid konumunu kontrol et
    public int GetWidth() => gridSystem.GetWidth();  // Aðýrlýðýn geniþliðini al
    public int GetHeight() => gridSystem.GetHeight();  // Aðýrlýðýn yüksekliðini al

    // Belirli bir hücre konumunda herhangi bir birim olup olmadýðýný kontrol eder.
    public bool HasAnyUnitOnGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.HasAnyUnit();
    }

    // Belirli bir hücre konumunda bulunan bir birimi alýr.
    public Unit GetUnitAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetUnit();
    }

    // Belirli bir hücre konumunda etkileþilebilir bir nesne olup olmadýðýný kontrol eder.
    public IInteractable GetInteractableAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetInteractable();
    }

    // Belirli bir hücre konumuna etkileþilebilir bir nesne ekler.
    public void SetInteractableAtGridPosition(GridPosition gridPosition, IInteractable interactable)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.SetInteractable(interactable);
    }

    // Belirli bir hücre konumunda olan etkileþilebilir nesneyi temizler.
    public void ClearInteractableAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.ClearInteractable();
    }
}
