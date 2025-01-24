using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    // PathNode sýnýfý, bir hücrenin bilgilerini temsil eder.

    private GridPosition gridPosition;   // Bu PathNode'nun konum bilgisi
    private int gCost;                    // Hareket maliyeti (gCost: grid cost)
    private int hCost;                    // Hedefe olan mesafe maliyeti (hCost: heuristic cost)
    private int fCost;                    // Toplam maliyet (fCost: toplam maliyet)
    private PathNode cameFromPathNode;    // Bu noddan nasýl ulaþýldýðýný saklar (önceki nodeler)
    private bool isWalkable = true;       // Bu hücrenin yürünebilir olup olmadýðýný belirler

    // PathNode sýnýfý için bir yapýlandýrýcý
    public PathNode(GridPosition gridPosition)
    {
        this.gridPosition = gridPosition;   // Grid konumunu ayarla
    }

    public override string ToString()
    {
        return gridPosition.ToString();   // Grid konumunu string olarak döndürür
    }

    public int GetGCost()
    {
        return gCost;   // gCost (Hareket maliyeti) deðerini döndürür
    }

    public int GetHCost()
    {
        return hCost;   // hCost (Heuristic maliyeti) deðerini döndürür
    }

    public int GetFCost()
    {
        return fCost;   // fCost (Toplam maliyet) deðerini döndürür
    }

    public void SetGCost(int gCost)
    {
        this.gCost = gCost;   // gCost (Hareket maliyeti) deðerini ayarlar
    }

    public void SetHCost(int hCost)
    {
        this.hCost = hCost;   // hCost (Heuristic maliyeti) deðerini ayarlar
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;   // fCost hesaplanýr: gCost + hCost
    }

    public void ResetCameFromPathNode()
    {
        cameFromPathNode = null;   // cameFromPathNode'yi sýfýrlar
    }

    public void SetCameFromPathNode(PathNode pathNode)
    {
        cameFromPathNode = pathNode;   // cameFromPathNode'yi belirtilen PathNode olarak ayarlar
    }

    public PathNode GetCameFromPathNode()
    {
        return cameFromPathNode;   // cameFromPathNode deðerini döndürür
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;   // Bu PathNode'nun Grid konumunu döndürür
    }

    public bool IsWalkable()
    {
        return isWalkable;   // Bu PathNode'nun yürünebilir olup olmadýðýný döndürür
    }

    public void SetIsWalkable(bool isWalkable)
    {
        this.isWalkable = isWalkable;   // Bu PathNode'nun yürünebilir olup olmadýðýný ayarlar
    }
}
