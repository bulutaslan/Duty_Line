using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingUpdater : MonoBehaviour
{
    private void Start()
    {
        // DestructibleCrate nesnesinin yok edilmesi durumunda ger�ekle�en i�lemi dinle
        DestructibleCrate.OnAnyDestroyed += DestructibleCrate_OnAnyDestroyed;
    }

    // DestructibleCrate nesnesi yok edildi�inde g�ncellenir
    private void DestructibleCrate_OnAnyDestroyed(object sender, EventArgs e)
    {
        DestructibleCrate destructibleCrate = sender as DestructibleCrate;
        Pathfinding.Instance.SetIsWalkableGridPosition(destructibleCrate.GetGridPosition(), true);  // Yok edilen nesnenin bulundu�u noktay� y�r�nebilir yap
    }
}
