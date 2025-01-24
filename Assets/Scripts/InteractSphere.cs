using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractSphere : MonoBehaviour, IInteractable
{
    [SerializeField] private Material greenMaterial;  // Yeþil materyal
    [SerializeField] private Material redMaterial;    // Kýrmýzý materyal
    [SerializeField] private MeshRenderer meshRenderer;  // MeshRenderer bileþeni

    private GridPosition gridPosition;  // Grid üzerindeki konum
    private bool isGreen;  // Renk durumu (yeþil mi deðil mi?)
    private Action onInteractionComplete;  // Etkileþim tamamlandýðýnda tetiklenecek iþ
    private bool isActive;  // Etkileþim aktif mi?
    private float timer;  // Etkileþim süresi

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);  // Grid pozisyonunu belirle
        LevelGrid.Instance.SetInteractableAtGridPosition(gridPosition, this);  // Grid pozisyonuna etkileþim yapabilen nesneyi ayarla

        SetColorGreen();  // Baþlangýçta rengi yeþil yap
    }

    private void Update()
    {
        if (!isActive)  // Etkileþim aktif deðilse iþlemi durdur
        {
            return;
        }

        timer -= Time.deltaTime;  // Timerý azalt

        if (timer <= 0f)  // Timer sýfýr veya altýna düþtüyse
        {
            isActive = false;
            onInteractionComplete();  // Etkileþim tamamlandý iþlemini tetikle
        }
    }

    private void SetColorGreen()
    {
        isGreen = true;
        meshRenderer.material = greenMaterial;  // Renk yeþil materyali olarak ayarla
    }

    private void SetColorRed()
    {
        isGreen = false;
        meshRenderer.material = redMaterial;  // Renk kýrmýzý materyali olarak ayarla
    }

    public void Interact(Action onInteractionComplete)
    {
        this.onInteractionComplete = onInteractionComplete;
        isActive = true;
        timer = .5f;  // Timer süresini ayarla

        if (isGreen)
        {
            SetColorRed();  // Renk kýrmýzýya dönüþtür
        }
        else
        {
            SetColorGreen();  // Renk yeþile dönüþtür
        }
    }
}
