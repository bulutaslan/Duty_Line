using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    // Singleton �rne�i, LevelGrid s�n�f�n�n sadece bir �rne�inin varl���n� sa�lar.
    public static LevelGrid Instance { get; private set; }

    // Oyun alan�nda herhangi bir birimin konum de�i�ikli�ini izleyen olay.
    public event EventHandler<OnAnyUnitMovedGridPositionEventArgs> OnAnyUnitMovedGridPosition;

    public class OnAnyUnitMovedGridPositionEventArgs : EventArgs
    {
        public Unit unit;                  // Birim nesnesi
        public GridPosition fromGridPosition;  // Birimin ba�lad��� h�cre konumu
        public GridPosition toGridPosition;    // Birimin gitti�i h�cre konumu
    }

    [SerializeField] private Transform gridDebugObjectPrefab;  // A��, h�creleri g�rselle�tirmek i�in kullan�lacak nesne
    [SerializeField] private int width;  // A��rl�k geni�li�i
    [SerializeField] private int height; // A�� geni�li�i
    [SerializeField] private float cellSize; // H�cre boyutu

    private GridSystem<GridObject> gridSystem; // A�� i�lemek i�in GridSystem nesnesi

    // Her oyun nesnesi uyanmadan �nce �a�r�lan metod
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("LevelGrid �rne�i zaten mevcut! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // Grid sistemini olu�turuyoruz ve her h�crede GridObject nesnesini tutuyoruz.
        gridSystem = new GridSystem<GridObject>(width, height, cellSize,
            (GridSystem<GridObject> g, GridPosition gridPosition) => new GridObject(g, gridPosition));
        // gridSystem.CreateDebugObjects(gridDebugObjectPrefab); // Debug nesneleri olu�turulabilir
    }

    private void Start()
    {
        // A��rl�k sistemi i�in ba�lang�� ayarlar�n� yap�yoruz.
        Pathfinding.Instance.Setup(width, height, cellSize);
    }

    // H�cre konumuna bir birimi ekler.
    public void AddUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.AddUnit(unit);
    }

    // Belirli bir h�cre konumunda bulunan t�m birimleri d�ner.
    public List<Unit> GetUnitListAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetUnitList();
    }

    // Belirli bir h�cre konumundan bir birimi kald�r�r.
    public void RemoveUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.RemoveUnit(unit);
    }

    // Bir birimin h�cre konumunu g�nceller ve olay tetiklenir.
    public void UnitMovedGridPosition(Unit unit, GridPosition fromGridPosition, GridPosition toGridPosition)
    {
        RemoveUnitAtGridPosition(fromGridPosition, unit);  // �nce birimi kald�r
        AddUnitAtGridPosition(toGridPosition, unit);       // Sonra yeni konumda ekle

        // Konum de�i�ikli�ini dinleyicilere bildir
        OnAnyUnitMovedGridPosition?.Invoke(this, new OnAnyUnitMovedGridPositionEventArgs
        {
            unit = unit,
            fromGridPosition = fromGridPosition,
            toGridPosition = toGridPosition,
        });
    }

    public GridPosition GetGridPosition(Vector3 worldPosition) => gridSystem.GetGridPosition(worldPosition);  // D�nyadan grid konumunu al
    public Vector3 GetWorldPosition(GridPosition gridPosition) => gridSystem.GetWorldPosition(gridPosition);      // Grid konumundan d�nyaya pozisyon al
    public bool IsValidGridPosition(GridPosition gridPosition) => gridSystem.IsValidGridPosition(gridPosition); // Ge�erli grid konumunu kontrol et
    public int GetWidth() => gridSystem.GetWidth();  // A��rl���n geni�li�ini al
    public int GetHeight() => gridSystem.GetHeight();  // A��rl���n y�ksekli�ini al

    // Belirli bir h�cre konumunda herhangi bir birim olup olmad���n� kontrol eder.
    public bool HasAnyUnitOnGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.HasAnyUnit();
    }

    // Belirli bir h�cre konumunda bulunan bir birimi al�r.
    public Unit GetUnitAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetUnit();
    }

    // Belirli bir h�cre konumunda etkile�ilebilir bir nesne olup olmad���n� kontrol eder.
    public IInteractable GetInteractableAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetInteractable();
    }

    // Belirli bir h�cre konumuna etkile�ilebilir bir nesne ekler.
    public void SetInteractableAtGridPosition(GridPosition gridPosition, IInteractable interactable)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.SetInteractable(interactable);
    }

    // Belirli bir h�cre konumunda olan etkile�ilebilir nesneyi temizler.
    public void ClearInteractableAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.ClearInteractable();
    }
}
