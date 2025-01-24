using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PathfindingGridDebugObject : GridDebugObject
{
    // G maliyeti, H maliyeti ve F maliyeti göstermek için UI elemanlarý
    [SerializeField] private TextMeshPro gCostText;
    [SerializeField] private TextMeshPro hCostText;
    [SerializeField] private TextMeshPro fCostText;
    [SerializeField] private SpriteRenderer isWalkableSpriteRenderer;

    private PathNode pathNode;

    public override void SetGridObject(object gridObject)
    {
        base.SetGridObject(gridObject);
        pathNode = (PathNode)gridObject;
    }

    // UI elemanlarýnýn güncellenmesi iþlemleri
    protected override void Update()
    {
        base.Update();
        gCostText.text = pathNode.GetGCost().ToString();  // G maliyetini güncelle
        hCostText.text = pathNode.GetHCost().ToString();  // H maliyetini güncelle
        fCostText.text = pathNode.GetFCost().ToString();  // F maliyetini güncelle
        isWalkableSpriteRenderer.color = pathNode.IsWalkable() ? Color.green : Color.red;  // Yürünebilirlik durumunu göster
    }
}
