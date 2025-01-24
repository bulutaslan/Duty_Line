using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBusyUI : MonoBehaviour
{
    // Aksiyonun meþgul durumunu göstermek için UI öðesini yöneten sýnýf

    private void Start()
    {
        // Aksiyon sistemindeki meþgul durumunu dinle
        UnitActionSystem.Instance.OnBusyChanged += UnitActionSystem_OnBusyChanged;

        Hide();   // UI baþlangýçta gizlensin
    }

    private void Show()
    {
        // UI öðesini aktif yap
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        // UI öðesini pasif yap
        gameObject.SetActive(false);
    }

    private void UnitActionSystem_OnBusyChanged(object sender, bool isBusy)
    {
        // Meþgul durum deðiþtiðinde UI'yi göster ya da gizle
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
