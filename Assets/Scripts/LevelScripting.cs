using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScripting : MonoBehaviour
{
    [SerializeField] private List<GameObject> hider1List;  // �lk hider listesi
    [SerializeField] private List<GameObject> hider2List;  // �kinci hider listesi
    [SerializeField] private List<GameObject> hider3List;  // ���nc� hider listesi
    [SerializeField] private List<GameObject> enemy1List;   // �lk d��man listesi
    [SerializeField] private List<GameObject> enemy2List;   // �kinci d��man listesi
    [SerializeField] private Door door1;                    // Birinci kap�
    [SerializeField] private Door door2;                    // �kinci kap�

    private bool hasShownFirstHider = false; // �lk hider'�n g�sterilip g�sterilmedi�ini kontrol eden bool de�i�ken

    private void Start()
    {
        // Konum de�i�ikliklerini dinlemek i�in LevelGrid'e ba�lan�r.
        LevelGrid.Instance.OnAnyUnitMovedGridPosition += LevelGrid_OnAnyUnitMovedGridPosition;

        // Kap� a��ld���nda ilgili hider ve d��man listesini a�-kapa yapar.
        door1.OnDoorOpened += (object sender, EventArgs e) =>
        {
            SetActiveGameObjectList(hider2List, false);  // Kap� 1 a��ld���nda ikinci hider listesi pasif olur
        };
        door2.OnDoorOpened += (object sender, EventArgs e) =>
        {
            SetActiveGameObjectList(hider3List, false);  // Kap� 2 a��ld���nda ���nc� hider listesi pasif olur
            SetActiveGameObjectList(enemy2List, true);    // Ve ikinci d��man listesi aktif olur
        };
    }

    // LevelGrid s�n�f�n�n OnAnyUnitMovedGridPosition olay�n� dinler.
    private void LevelGrid_OnAnyUnitMovedGridPosition(object sender, LevelGrid.OnAnyUnitMovedGridPositionEventArgs e)
    {
        // E�er birim z konumunda 5 ise ve bu ilk hider g�sterilmediyse
        if (e.toGridPosition.z == 5 && !hasShownFirstHider)
        {
            hasShownFirstHider = true;
            SetActiveGameObjectList(hider1List, false);  // �lk hider listesi kapal�
            SetActiveGameObjectList(enemy1List, true);   // �lk d��man listesi aktif
        }
    }

    // Bir liste i�indeki t�m nesnelerin etkinli�ini ayarlar.
    private void SetActiveGameObjectList(List<GameObject> gameObjectList, bool isActive)
    {
        foreach (GameObject gameObject in gameObjectList)
        {
            gameObject.SetActive(isActive);  // Her bir nesneyi aktif/pasif yap
        }
    }
}
