using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    // PathNode s�n�f�, bir h�crenin bilgilerini temsil eder.

    private GridPosition gridPosition;   // Bu PathNode'nun konum bilgisi
    private int gCost;                    // Hareket maliyeti (gCost: grid cost)
    private int hCost;                    // Hedefe olan mesafe maliyeti (hCost: heuristic cost)
    private int fCost;                    // Toplam maliyet (fCost: toplam maliyet)
    private PathNode cameFromPathNode;    // Bu noddan nas�l ula��ld���n� saklar (�nceki nodeler)
    private bool isWalkable = true;       // Bu h�crenin y�r�nebilir olup olmad���n� belirler

    // PathNode s�n�f� i�in bir yap�land�r�c�
    public PathNode(GridPosition gridPosition)
    {
        this.gridPosition = gridPosition;   // Grid konumunu ayarla
    }

    public override string ToString()
    {
        return gridPosition.ToString();   // Grid konumunu string olarak d�nd�r�r
    }

    public int GetGCost()
    {
        return gCost;   // gCost (Hareket maliyeti) de�erini d�nd�r�r
    }

    public int GetHCost()
    {
        return hCost;   // hCost (Heuristic maliyeti) de�erini d�nd�r�r
    }

    public int GetFCost()
    {
        return fCost;   // fCost (Toplam maliyet) de�erini d�nd�r�r
    }

    public void SetGCost(int gCost)
    {
        this.gCost = gCost;   // gCost (Hareket maliyeti) de�erini ayarlar
    }

    public void SetHCost(int hCost)
    {
        this.hCost = hCost;   // hCost (Heuristic maliyeti) de�erini ayarlar
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;   // fCost hesaplan�r: gCost + hCost
    }

    public void ResetCameFromPathNode()
    {
        cameFromPathNode = null;   // cameFromPathNode'yi s�f�rlar
    }

    public void SetCameFromPathNode(PathNode pathNode)
    {
        cameFromPathNode = pathNode;   // cameFromPathNode'yi belirtilen PathNode olarak ayarlar
    }

    public PathNode GetCameFromPathNode()
    {
        return cameFromPathNode;   // cameFromPathNode de�erini d�nd�r�r
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;   // Bu PathNode'nun Grid konumunu d�nd�r�r
    }

    public bool IsWalkable()
    {
        return isWalkable;   // Bu PathNode'nun y�r�nebilir olup olmad���n� d�nd�r�r
    }

    public void SetIsWalkable(bool isWalkable)
    {
        this.isWalkable = isWalkable;   // Bu PathNode'nun y�r�nebilir olup olmad���n� ayarlar
    }
}
