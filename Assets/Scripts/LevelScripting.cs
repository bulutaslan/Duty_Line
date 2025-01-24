using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScripting : MonoBehaviour
{
    [SerializeField] private List<GameObject> hider1List;  // Ýlk hider listesi
    [SerializeField] private List<GameObject> hider2List;  // Ýkinci hider listesi
    [SerializeField] private List<GameObject> hider3List;  // Üçüncü hider listesi
    [SerializeField] private List<GameObject> enemy1List;   // Ýlk düþman listesi
    [SerializeField] private List<GameObject> enemy2List;   // Ýkinci düþman listesi
    [SerializeField] private Door door1;                    // Birinci kapý
    [SerializeField] private Door door2;                    // Ýkinci kapý

    private bool hasShownFirstHider = false; // Ýlk hider'ýn gösterilip gösterilmediðini kontrol eden bool deðiþken

    private void Start()
    {
        // Konum deðiþikliklerini dinlemek için LevelGrid'e baðlanýr.
        LevelGrid.Instance.OnAnyUnitMovedGridPosition += LevelGrid_OnAnyUnitMovedGridPosition;

        // Kapý açýldýðýnda ilgili hider ve düþman listesini aç-kapa yapar.
        door1.OnDoorOpened += (object sender, EventArgs e) =>
        {
            SetActiveGameObjectList(hider2List, false);  // Kapý 1 açýldýðýnda ikinci hider listesi pasif olur
        };
        door2.OnDoorOpened += (object sender, EventArgs e) =>
        {
            SetActiveGameObjectList(hider3List, false);  // Kapý 2 açýldýðýnda üçüncü hider listesi pasif olur
            SetActiveGameObjectList(enemy2List, true);    // Ve ikinci düþman listesi aktif olur
        };
    }

    // LevelGrid sýnýfýnýn OnAnyUnitMovedGridPosition olayýný dinler.
    private void LevelGrid_OnAnyUnitMovedGridPosition(object sender, LevelGrid.OnAnyUnitMovedGridPositionEventArgs e)
    {
        // Eðer birim z konumunda 5 ise ve bu ilk hider gösterilmediyse
        if (e.toGridPosition.z == 5 && !hasShownFirstHider)
        {
            hasShownFirstHider = true;
            SetActiveGameObjectList(hider1List, false);  // Ýlk hider listesi kapalý
            SetActiveGameObjectList(enemy1List, true);   // Ýlk düþman listesi aktif
        }
    }

    // Bir liste içindeki tüm nesnelerin etkinliðini ayarlar.
    private void SetActiveGameObjectList(List<GameObject> gameObjectList, bool isActive)
    {
        foreach (GameObject gameObject in gameObjectList)
        {
            gameObject.SetActive(isActive);  // Her bir nesneyi aktif/pasif yap
        }
    }
}
