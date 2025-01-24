using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractSphere : MonoBehaviour, IInteractable
{
    [SerializeField] private Material greenMaterial;  // Ye�il materyal
    [SerializeField] private Material redMaterial;    // K�rm�z� materyal
    [SerializeField] private MeshRenderer meshRenderer;  // MeshRenderer bile�eni

    private GridPosition gridPosition;  // Grid �zerindeki konum
    private bool isGreen;  // Renk durumu (ye�il mi de�il mi?)
    private Action onInteractionComplete;  // Etkile�im tamamland���nda tetiklenecek i�
    private bool isActive;  // Etkile�im aktif mi?
    private float timer;  // Etkile�im s�resi

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);  // Grid pozisyonunu belirle
        LevelGrid.Instance.SetInteractableAtGridPosition(gridPosition, this);  // Grid pozisyonuna etkile�im yapabilen nesneyi ayarla

        SetColorGreen();  // Ba�lang��ta rengi ye�il yap
    }

    private void Update()
    {
        if (!isActive)  // Etkile�im aktif de�ilse i�lemi durdur
        {
            return;
        }

        timer -= Time.deltaTime;  // Timer� azalt

        if (timer <= 0f)  // Timer s�f�r veya alt�na d��t�yse
        {
            isActive = false;
            onInteractionComplete();  // Etkile�im tamamland� i�lemini tetikle
        }
    }

    private void SetColorGreen()
    {
        isGreen = true;
        meshRenderer.material = greenMaterial;  // Renk ye�il materyali olarak ayarla
    }

    private void SetColorRed()
    {
        isGreen = false;
        meshRenderer.material = redMaterial;  // Renk k�rm�z� materyali olarak ayarla
    }

    public void Interact(Action onInteractionComplete)
    {
        this.onInteractionComplete = onInteractionComplete;
        isActive = true;
        timer = .5f;  // Timer s�resini ayarla

        if (isGreen)
        {
            SetColorRed();  // Renk k�rm�z�ya d�n��t�r
        }
        else
        {
            SetColorGreen();  // Renk ye�ile d�n��t�r
        }
    }
}
