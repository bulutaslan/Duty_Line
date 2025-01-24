using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PathfindingGridDebugObject : GridDebugObject
{
    // G maliyeti, H maliyeti ve F maliyeti g�stermek i�in UI elemanlar�
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

    // UI elemanlar�n�n g�ncellenmesi i�lemleri
    protected override void Update()
    {
        base.Update();
        gCostText.text = pathNode.GetGCost().ToString();  // G maliyetini g�ncelle
        hCostText.text = pathNode.GetHCost().ToString();  // H maliyetini g�ncelle
        fCostText.text = pathNode.GetFCost().ToString();  // F maliyetini g�ncelle
        isWalkableSpriteRenderer.color = pathNode.IsWalkable() ? Color.green : Color.red;  // Y�r�nebilirlik durumunu g�ster
    }
}
