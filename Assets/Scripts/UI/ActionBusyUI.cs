using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBusyUI : MonoBehaviour
{
    // Aksiyonun me�gul durumunu g�stermek i�in UI ��esini y�neten s�n�f

    private void Start()
    {
        // Aksiyon sistemindeki me�gul durumunu dinle
        UnitActionSystem.Instance.OnBusyChanged += UnitActionSystem_OnBusyChanged;

        Hide();   // UI ba�lang��ta gizlensin
    }

    private void Show()
    {
        // UI ��esini aktif yap
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        // UI ��esini pasif yap
        gameObject.SetActive(false);
    }

    private void UnitActionSystem_OnBusyChanged(object sender, bool isBusy)
    {
        // Me�gul durum de�i�ti�inde UI'yi g�ster ya da gizle
        if (isBusy)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }
}
