using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    // Kap�n�n a��ld��� olay
    public static event EventHandler OnAnyDoorOpened;
    public event EventHandler OnDoorOpened;

    [SerializeField] private bool isOpen;  // Kap�n�n a��k olup olmad���n� kontrol eden de�i�ken

    private GridPosition gridPosition;    // Kap�n�n bulundu�u a�daki konum
    private Animator animator;           // Kap�n�n animasyonlar�n� kontrol eden bile�en
    private Action onInteractionComplete; // Etkile�im tamamland���nda �a�r�lacak i�lem
    private bool isActive;               // Kap�n�n aktif olup olmad���n� belirten de�i�ken
    private float timer;                 // Kap�n�n ge�ici bir s�re etkin oldu�unu g�steren zamanlay�c�

    // Kap�n�n ba�lang��ta haz�rland��� yer
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetInteractableAtGridPosition(gridPosition, this);

        // Kap� ba�lang��ta a��k m� yoksa kapal� m� kontrol et
        if (isOpen)
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }
    }

    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        // Kap� etkile�im s�re kontrol�
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            isActive = false;
            onInteractionComplete();
        }
    }

    // Kap�y� a�mak veya kapatmak i�in yap�lan etkile�im
    public void Interact(Action onInteractionComplete)
    {
        this.onInteractionComplete = onInteractionComplete;
        isActive = true;
        timer = .5f;

        if (isOpen)
        {
            CloseDoor();
        }
        else
        {
            OpenDoor();
        }
    }

    // Kap�y� a�ma i�lemi
    private void OpenDoor()
    {
        isOpen = true;
        animator.SetBool("IsOpen", isOpen);  // Animasyonda kap�n�n a��k oldu�unu belirten parametreyi ayarla
        Pathfinding.Instance.SetIsWalkableGridPosition(gridPosition, true);  // Kap�n�n konumunda y�r�nebilir hale getir

        OnDoorOpened?.Invoke(this, EventArgs.Empty);  // Kap�n�n a��ld���n� bildir
        OnAnyDoorOpened?.Invoke(this, EventArgs.Empty);
    }

    // Kap�y� kapama i�lemi
    private void CloseDoor()
    {
        isOpen = false;
        animator.SetBool("IsOpen", isOpen);  // Animasyonda kap�n�n kapal� oldu�unu belirten parametreyi ayarla
        Pathfinding.Instance.SetIsWalkableGridPosition(gridPosition, false);  // Kap�n�n konumunda y�r�nebilirli�i kald�r
    }
}
