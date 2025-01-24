using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    [SerializeField] private GameObject actionCameraGameObject;  // Etkile�im kameras�

    private void Start()
    {
        BaseAction.OnAnyActionStarted += BaseAction_OnAnyActionStarted;  // Herhangi bir aksiyon ba�larsa tetiklenecek
        BaseAction.OnAnyActionCompleted += BaseAction_OnAnyActionCompleted;  // Herhangi bir aksiyon tamamlan�rsa tetiklenecek

        HideActionCamera();  // Etkile�im kameras�n� ba�lang��ta gizle
    }


    private void ShowActionCamera()
    {
        if (actionCameraGameObject == null)
        {
            Debug.LogError("Aksiyon Kameras� nesnesi eksik. Bulmaya veya olu�turmaya �al���l�yor.");  // Hata mesaj�
            // Burada yeni bir kamera nesnesi olu�turulabilir veya uygun bir nesne bulunabilir.
            // �rne�in, sahnedeki bir nesneyi bulmak:
            actionCameraGameObject = GameObject.Find("ActionCamera");

            // Veya yeni bir kamera olu�turmak:
            if (actionCameraGameObject == null)
            {
                actionCameraGameObject = new GameObject("ActionCamera");
                actionCameraGameObject.AddComponent<Camera>();  // Burada Camera komutunu ekle
            }
        }

        actionCameraGameObject.SetActive(true);  // Aksiyon kameras�n� aktif et
    }

    private void HideActionCamera()
    {
        if (actionCameraGameObject == null)
        {
            Debug.LogError("Aksiyon Kameras� nesnesi eksik.");  // Hata mesaj�
            return;
        }

        actionCameraGameObject.SetActive(false);  // Aksiyon kameras�n� kapat
    }


    private void BaseAction_OnAnyActionStarted(object sender, EventArgs e)
    {
        if (actionCameraGameObject == null)
        {
            actionCameraGameObject = GameObject.Find("ActionCamera");  // Aksiyon kameras�n� bul
            if (actionCameraGameObject == null)  // E�er bulunamazsa hata mesaj� ver
            {
                Debug.LogError("Aksiyon Kameras� nesnesi eksik.");
                return;
            }
        }

        switch (sender)
        {
            case ShootAction shootAction:  // F�rlat�lan bir mermi
                Unit shooterUnit = shootAction.GetUnit();  // At��� yapan birim
                Unit targetUnit = shootAction.GetTargetUnit();  // Hedef al�nan birim

                Vector3 cameraCharacterHeight = Vector3.up * 1.7f;  // Kamera ve karakter aras�ndaki y�ksekli�i ayarla

                Vector3 shootDir = (targetUnit.GetWorldPosition() - shooterUnit.GetWorldPosition()).normalized;  // At�� y�n�n� hesapla

                float shoulderOffsetAmount = 0.5f;  // Omuz ofset miktar�
                Vector3 shoulderOffset = Quaternion.Euler(0, 90, 0) * shootDir * shoulderOffsetAmount;  // Omuz ofsetini hesapla

                Vector3 actionCameraPosition =  // Aksiyon kameran�n pozisyonunu hesapla
                    shooterUnit.GetWorldPosition() +
                    cameraCharacterHeight +
                    shoulderOffset +
                    (shootDir * -1);

                actionCameraGameObject.transform.position = actionCameraPosition;
                actionCameraGameObject.transform.LookAt(targetUnit.GetWorldPosition() + cameraCharacterHeight);

                ShowActionCamera();  // Aksiyon kameras�n� g�ster
                break;
        }
    }


    private void BaseAction_OnAnyActionCompleted(object sender, EventArgs e)
    {
        if (actionCameraGameObject == null)
        {
            // Sahne y�klendikten sonra Aksiyon Kamera nesnesini tekrar bul
            actionCameraGameObject = GameObject.Find("ActionCamera");
            if (actionCameraGameObject == null)  // H�l� bulunamazsa hata ver ve ��k
            {
                Debug.LogError("Aksiyon Kameras� nesnesi eksik.");
                return;
            }
        }

        switch (sender)
        {
            case ShootAction shootAction:  // At�lan mermi
                HideActionCamera();  // Aksiyon kameras�n� gizle
                break;
        }
    }
}