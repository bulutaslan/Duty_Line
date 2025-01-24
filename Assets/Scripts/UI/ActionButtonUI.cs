using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActionButtonUI : MonoBehaviour
{
    // Aksiyon butonlarýnýn görünümünü ve iþleyiþini yöneten sýnýf

    [SerializeField] private TextMeshProUGUI textMeshPro;   // Buton üzerinde gösterilecek metin
    [SerializeField] private Button button;   // Buton bileþeni
    [SerializeField] private GameObject selectedGameObject;   // Seçilen görselin aktif olduðu obje

    private BaseAction baseAction;

    public void SetBaseAction(BaseAction baseAction)
    {
        this.baseAction = baseAction;
        textMeshPro.text = baseAction.GetActionName().ToUpper();   // Aksiyon adýný büyük harflerle yazdýr

        button.onClick.AddListener(() => {
            // Butona týklandýðýnda seçilen aksiyonu ayarla
            UnitActionSystem.Instance.SetSelectedAction(baseAction);
        });
    }

    public void UpdateSelectedVisual()
    {
        // Aksiyonun seçili olup olmadýðýný görsel olarak güncelle
        BaseAction selectedBaseAction = UnitActionSystem.Instance.GetSelectedAction();
        selectedGameObject.SetActive(selectedBaseAction == baseAction);
    }
}
