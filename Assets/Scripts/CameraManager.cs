using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    [SerializeField] private GameObject actionCameraGameObject;  // Etkileþim kamerasý

    private void Start()
    {
        BaseAction.OnAnyActionStarted += BaseAction_OnAnyActionStarted;  // Herhangi bir aksiyon baþlarsa tetiklenecek
        BaseAction.OnAnyActionCompleted += BaseAction_OnAnyActionCompleted;  // Herhangi bir aksiyon tamamlanýrsa tetiklenecek

        HideActionCamera();  // Etkileþim kamerasýný baþlangýçta gizle
    }


    private void ShowActionCamera()
    {
        if (actionCameraGameObject == null)
        {
            Debug.LogError("Aksiyon Kamerasý nesnesi eksik. Bulmaya veya oluþturmaya çalýþýlýyor.");  // Hata mesajý
            // Burada yeni bir kamera nesnesi oluþturulabilir veya uygun bir nesne bulunabilir.
            // Örneðin, sahnedeki bir nesneyi bulmak:
            actionCameraGameObject = GameObject.Find("ActionCamera");

            // Veya yeni bir kamera oluþturmak:
            if (actionCameraGameObject == null)
            {
                actionCameraGameObject = new GameObject("ActionCamera");
                actionCameraGameObject.AddComponent<Camera>();  // Burada Camera komutunu ekle
            }
        }

        actionCameraGameObject.SetActive(true);  // Aksiyon kamerasýný aktif et
    }

    private void HideActionCamera()
    {
        if (actionCameraGameObject == null)
        {
            Debug.LogError("Aksiyon Kamerasý nesnesi eksik.");  // Hata mesajý
            return;
        }

        actionCameraGameObject.SetActive(false);  // Aksiyon kamerasýný kapat
    }


    private void BaseAction_OnAnyActionStarted(object sender, EventArgs e)
    {
        if (actionCameraGameObject == null)
        {
            actionCameraGameObject = GameObject.Find("ActionCamera");  // Aksiyon kamerasýný bul
            if (actionCameraGameObject == null)  // Eðer bulunamazsa hata mesajý ver
            {
                Debug.LogError("Aksiyon Kamerasý nesnesi eksik.");
                return;
            }
        }

        switch (sender)
        {
            case ShootAction shootAction:  // Fýrlatýlan bir mermi
                Unit shooterUnit = shootAction.GetUnit();  // Atýþý yapan birim
                Unit targetUnit = shootAction.GetTargetUnit();  // Hedef alýnan birim

                Vector3 cameraCharacterHeight = Vector3.up * 1.7f;  // Kamera ve karakter arasýndaki yüksekliði ayarla

                Vector3 shootDir = (targetUnit.GetWorldPosition() - shooterUnit.GetWorldPosition()).normalized;  // Atýþ yönünü hesapla

                float shoulderOffsetAmount = 0.5f;  // Omuz ofset miktarý
                Vector3 shoulderOffset = Quaternion.Euler(0, 90, 0) * shootDir * shoulderOffsetAmount;  // Omuz ofsetini hesapla

                Vector3 actionCameraPosition =  // Aksiyon kameranýn pozisyonunu hesapla
                    shooterUnit.GetWorldPosition() +
                    cameraCharacterHeight +
                    shoulderOffset +
                    (shootDir * -1);

                actionCameraGameObject.transform.position = actionCameraPosition;
                actionCameraGameObject.transform.LookAt(targetUnit.GetWorldPosition() + cameraCharacterHeight);

                ShowActionCamera();  // Aksiyon kamerasýný göster
                break;
        }
    }


    private void BaseAction_OnAnyActionCompleted(object sender, EventArgs e)
    {
        if (actionCameraGameObject == null)
        {
            // Sahne yüklendikten sonra Aksiyon Kamera nesnesini tekrar bul
            actionCameraGameObject = GameObject.Find("ActionCamera");
            if (actionCameraGameObject == null)  // Hâlâ bulunamazsa hata ver ve çýk
            {
                Debug.LogError("Aksiyon Kamerasý nesnesi eksik.");
                return;
            }
        }

        switch (sender)
        {
            case ShootAction shootAction:  // Atýlan mermi
                HideActionCamera();  // Aksiyon kamerasýný gizle
                break;
        }
    }
}