using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActionButtonUI : MonoBehaviour
{
    // Aksiyon butonlar�n�n g�r�n�m�n� ve i�leyi�ini y�neten s�n�f

    [SerializeField] private TextMeshProUGUI textMeshPro;   // Buton �zerinde g�sterilecek metin
    [SerializeField] private Button button;   // Buton bile�eni
    [SerializeField] private GameObject selectedGameObject;   // Se�ilen g�rselin aktif oldu�u obje

    private BaseAction baseAction;

    public void SetBaseAction(BaseAction baseAction)
    {
        this.baseAction = baseAction;
        textMeshPro.text = baseAction.GetActionName().ToUpper();   // Aksiyon ad�n� b�y�k harflerle yazd�r

        button.onClick.AddListener(() => {
            // Butona t�kland���nda se�ilen aksiyonu ayarla
            UnitActionSystem.Instance.SetSelectedAction(baseAction);
        });
    }

    public void UpdateSelectedVisual()
    {
        // Aksiyonun se�ili olup olmad���n� g�rsel olarak g�ncelle
        BaseAction selectedBaseAction = UnitActionSystem.Instance.GetSelectedAction();
        selectedGameObject.SetActive(selectedBaseAction == baseAction);
    }
}
