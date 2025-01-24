using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{
    // Grid sistemi �zerindeki g�rselli�i y�neten s�n�ft�r.

    public static GridSystemVisual Instance { get; private set; }   // Bu s�n�f�n tekil instance'�n� al�r

    [Serializable]
    public struct GridVisualTypeMaterial
    {
        public GridVisualType gridVisualType;   // Grid g�rsel t�r�
        public Material material;   // Bu g�rsel t�r�ne ait materyal
    }

    public enum GridVisualType
    {
        White,   // Beyaz g�rsel t�r�
        Blue,    // Mavi g�rsel t�r�
        Red,     // K�rm�z� g�rsel t�r�
        RedSoft, // Yumu�ak k�rm�z� g�rsel t�r�
        Yellow,  // Sar� g�rsel t�r�
    }

    [SerializeField] private Transform gridSystemVisualSinglePrefab;   // Tekil grid g�rselli�i prefab'�
    [SerializeField] private List<GridVisualTypeMaterial> gridVisualTypeMaterialList;   // G�rsel t�rleri ve materyalleri listesi

    private GridSystemVisualSingle[,] gridSystemVisualSingleArray;   // G�rsellik arrayi

    private void Awake()
    {
        // Bu s�n�f tekil bir instance m� yoksa birden fazla m�? Kontrol edilir
        if (Instance != null)
        {
            Debug.LogError("There's more than one GridSystemVisual! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;   // Tekil instance olarak bu nesneyi belirle
    }

    private void Start()
    {
        // Grid sisteminin geni�li�i ve y�ksekli�ine g�re g�rsellik array'ini olu�tur
        gridSystemVisualSingleArray = new GridSystemVisualSingle[
            LevelGrid.Instance.GetWidth(),
            LevelGrid.Instance.GetHeight()
        ];

        // Her bir grid h�cresine g�rsellik ekle
        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
        {
            for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);

                // G�rsel i�in prefab'� olu�tur ve yerle�tir
                Transform gridSystemVisualSingleTransform =
                    Instantiate(gridSystemVisualSinglePrefab, LevelGrid.Instance.GetWorldPosition(gridPosition), Quaternion.identity);

                gridSystemVisualSingleArray[x, z] = gridSystemVisualSingleTransform.GetComponent<GridSystemVisualSingle>();
            }
        }

        // Aksiyon sistemindeki olaylar� dinler
        UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
        LevelGrid.Instance.OnAnyUnitMovedGridPosition += LevelGrid_OnAnyUnitMovedGridPosition;

        UpdateGridVisual();   // Ba�lang��ta grid g�rselli�ini g�ncelle
    }

    public void HideAllGridPosition()
    {
        // T�m grid pozisyonlar�ndaki g�rsellikleri gizle
        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
        {
            for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
            {
                gridSystemVisualSingleArray[x, z].Hide();
            }
        }
    }

    private void ShowGridPositionRange(GridPosition gridPosition, int range, GridVisualType gridVisualType)
    {
        // Belirtilen bir alan i�indeki grid pozisyonlar�n� g�sterir
        List<GridPosition> gridPositionList = new List<GridPosition>();

        for (int x = -range; x <= range; x++)
        {
            for (int z = -range; z <= range; z++)
            {
                GridPosition testGridPosition = gridPosition + new GridPosition(x, z);

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > range)
                {
                    continue;
                }

                gridPositionList.Add(testGridPosition);
            }
        }

        ShowGridPositionList(gridPositionList, gridVisualType);
    }

    private void ShowGridPositionRangeSquare(GridPosition gridPosition, int range, GridVisualType gridVisualType)
    {
        // Belirtilen kareli alan i�indeki grid pozisyonlar�n� g�sterir
        List<GridPosition> gridPositionList = new List<GridPosition>();

        for (int x = -range; x <= range; x++)
        {
            for (int z = -range; z <= range; z++)
            {
                GridPosition testGridPosition = gridPosition + new GridPosition(x, z);

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }

                gridPositionList.Add(testGridPosition);
            }
        }

        ShowGridPositionList(gridPositionList, gridVisualType);
    }

    public void ShowGridPositionList(List<GridPosition> gridPositionList, GridVisualType gridVisualType)
    {
        // Belirtilen grid pozisyonlar�n� g�sterir
        foreach (GridPosition gridPosition in gridPositionList)
        {
            gridSystemVisualSingleArray[gridPosition.x, gridPosition.z].
                Show(GetGridVisualTypeMaterial(gridVisualType));   // G�rsel materyali ile h�creyi g�ster
        }
    }

    private void UpdateGridVisual()
    {
        HideAllGridPosition();   // T�m g�rselleri gizle

        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();   // Se�ilen birimi al
        BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction();   // Se�ilen aksiyonu al

        GridVisualType gridVisualType;

        switch (selectedAction)   // Se�ilen aksiyona g�re g�rsel tipi belirle
        {
            default:
            case MoveAction moveAction:
                gridVisualType = GridVisualType.White;
                break;
            case SpinAction spinAction:
                gridVisualType = GridVisualType.Blue;
                break;
            case ShootAction shootAction:
                gridVisualType = GridVisualType.Red;

                // Ni�an alma menzilini g�ster
                ShowGridPositionRange(selectedUnit.GetGridPosition(), shootAction.GetMaxShootDistance(), GridVisualType.RedSoft);
                break;
            case GrenadeAction grenadeAction:
                gridVisualType = GridVisualType.Yellow;
                break;
            case SwordAction swordAction:
                gridVisualType = GridVisualType.Red;

                // K�l�� menzilini kareli alan olarak g�ster
                ShowGridPositionRangeSquare(selectedUnit.GetGridPosition(), swordAction.GetMaxSwordDistance(), GridVisualType.RedSoft);
                break;
            case InteractAction interactAction:
                gridVisualType = GridVisualType.Blue;
                break;
        }

        // Ge�erli aksiyona g�re ge�erli grid pozisyonlar�n� g�ster
        ShowGridPositionList(
            selectedAction.GetValidActionGridPositionList(), gridVisualType);
    }

    private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e)
    {
        UpdateGridVisual();   // Se�ilen aksiyon de�i�ti�inde grid g�rselli�ini g�ncelle
    }

    private void LevelGrid_OnAnyUnitMovedGridPosition(object sender, EventArgs e)
    {
        UpdateGridVisual();   // Birim grid pozisyonu de�i�ti�inde g�rselli�i g�ncelle
    }

    private Material GetGridVisualTypeMaterial(GridVisualType gridVisualType)
    {
        // Belirtilen g�rsel t�r�ne ait materyali d�ner
        foreach (GridVisualTypeMaterial gridVisualTypeMaterial in gridVisualTypeMaterialList)
        {
            if (gridVisualTypeMaterial.gridVisualType == gridVisualType)
            {
                return gridVisualTypeMaterial.material;   // Materyali d�nd�r
            }
        }

        Debug.LogError("Could not find GridVisualTypeMaterial for GridVisualType " + gridVisualType);
        return null;   // G�rsel t�r�ne ait materyal bulunamazsa null d�ner
    }
}
