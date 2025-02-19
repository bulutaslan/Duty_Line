using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingUpdater : MonoBehaviour
{
    private void Start()
    {
        // DestructibleCrate nesnesinin yok edilmesi durumunda gerçekleşen işlemi dinle
        DestructibleCrate.OnAnyDestroyed += DestructibleCrate_OnAnyDestroyed;
    }

    // DestructibleCrate nesnesi yok edildiğinde güncellenir
    private void DestructibleCrate_OnAnyDestroyed(object sender, EventArgs e)
    {
        DestructibleCrate destructibleCrate = sender as DestructibleCrate;
        Pathfinding.Instance.SetIsWalkableGridPosition(destructibleCrate.GetGridPosition(), true);  // Yok edilen nesnenin bulunduğu noktayı yürünebilir yap
    }
}
