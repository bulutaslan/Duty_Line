using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{
    // Grid sistemi üzerindeki görselliði yöneten sýnýftýr.

    public static GridSystemVisual Instance { get; private set; }   // Bu sýnýfýn tekil instance'ýný alýr

    [Serializable]
    public struct GridVisualTypeMaterial
    {
        public GridVisualType gridVisualType;   // Grid görsel türü
        public Material material;   // Bu görsel türüne ait materyal
    }

    public enum GridVisualType
    {
        White,   // Beyaz görsel türü
        Blue,    // Mavi görsel türü
        Red,     // Kýrmýzý görsel türü
        RedSoft, // Yumuþak kýrmýzý görsel türü
        Yellow,  // Sarý görsel türü
    }

    [SerializeField] private Transform gridSystemVisualSinglePrefab;   // Tekil grid görselliði prefab'ý
    [SerializeField] private List<GridVisualTypeMaterial> gridVisualTypeMaterialList;   // Görsel türleri ve materyalleri listesi

    private GridSystemVisualSingle[,] gridSystemVisualSingleArray;   // Görsellik arrayi

    private void Awake()
    {
        // Bu sýnýf tekil bir instance mý yoksa birden fazla mý? Kontrol edilir
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
        // Grid sisteminin geniþliði ve yüksekliðine göre görsellik array'ini oluþtur
        gridSystemVisualSingleArray = new GridSystemVisualSingle[
            LevelGrid.Instance.GetWidth(),
            LevelGrid.Instance.GetHeight()
        ];

        // Her bir grid hücresine görsellik ekle
        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
        {
            for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);

                // Görsel için prefab'ý oluþtur ve yerleþtir
                Transform gridSystemVisualSingleTransform =
                    Instantiate(gridSystemVisualSinglePrefab, LevelGrid.Instance.GetWorldPosition(gridPosition), Quaternion.identity);

                gridSystemVisualSingleArray[x, z] = gridSystemVisualSingleTransform.GetComponent<GridSystemVisualSingle>();
            }
        }

        // Aksiyon sistemindeki olaylarý dinler
        UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
        LevelGrid.Instance.OnAnyUnitMovedGridPosition += LevelGrid_OnAnyUnitMovedGridPosition;

        UpdateGridVisual();   // Baþlangýçta grid görselliðini güncelle
    }

    public void HideAllGridPosition()
    {
        // Tüm grid pozisyonlarýndaki görsellikleri gizle
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
        // Belirtilen bir alan içindeki grid pozisyonlarýný gösterir
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
        // Belirtilen kareli alan içindeki grid pozisyonlarýný gösterir
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
        // Belirtilen grid pozisyonlarýný gösterir
        foreach (GridPosition gridPosition in gridPositionList)
        {
            gridSystemVisualSingleArray[gridPosition.x, gridPosition.z].
                Show(GetGridVisualTypeMaterial(gridVisualType));   // Görsel materyali ile hücreyi göster
        }
    }

    private void UpdateGridVisual()
    {
        HideAllGridPosition();   // Tüm görselleri gizle

        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();   // Seçilen birimi al
        BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction();   // Seçilen aksiyonu al

        GridVisualType gridVisualType;

        switch (selectedAction)   // Seçilen aksiyona göre görsel tipi belirle
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

                // Niþan alma menzilini göster
                ShowGridPositionRange(selectedUnit.GetGridPosition(), shootAction.GetMaxShootDistance(), GridVisualType.RedSoft);
                break;
            case GrenadeAction grenadeAction:
                gridVisualType = GridVisualType.Yellow;
                break;
            case SwordAction swordAction:
                gridVisualType = GridVisualType.Red;

                // Kýlýç menzilini kareli alan olarak göster
                ShowGridPositionRangeSquare(selectedUnit.GetGridPosition(), swordAction.GetMaxSwordDistance(), GridVisualType.RedSoft);
                break;
            case InteractAction interactAction:
                gridVisualType = GridVisualType.Blue;
                break;
        }

        // Geçerli aksiyona göre geçerli grid pozisyonlarýný göster
        ShowGridPositionList(
            selectedAction.GetValidActionGridPositionList(), gridVisualType);
    }

    private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e)
    {
        UpdateGridVisual();   // Seçilen aksiyon deðiþtiðinde grid görselliðini güncelle
    }

    private void LevelGrid_OnAnyUnitMovedGridPosition(object sender, EventArgs e)
    {
        UpdateGridVisual();   // Birim grid pozisyonu deðiþtiðinde görselliði güncelle
    }

    private Material GetGridVisualTypeMaterial(GridVisualType gridVisualType)
    {
        // Belirtilen görsel türüne ait materyali döner
        foreach (GridVisualTypeMaterial gridVisualTypeMaterial in gridVisualTypeMaterialList)
        {
            if (gridVisualTypeMaterial.gridVisualType == gridVisualType)
            {
                return gridVisualTypeMaterial.material;   // Materyali döndür
            }
        }

        Debug.LogError("Could not find GridVisualTypeMaterial for GridVisualType " + gridVisualType);
        return null;   // Görsel türüne ait materyal bulunamazsa null döner
    }
}
