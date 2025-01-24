using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    // Kapýnýn açýldýðý olay
    public static event EventHandler OnAnyDoorOpened;
    public event EventHandler OnDoorOpened;

    [SerializeField] private bool isOpen;  // Kapýnýn açýk olup olmadýðýný kontrol eden deðiþken

    private GridPosition gridPosition;    // Kapýnýn bulunduðu aðdaki konum
    private Animator animator;           // Kapýnýn animasyonlarýný kontrol eden bileþen
    private Action onInteractionComplete; // Etkileþim tamamlandýðýnda çaðrýlacak iþlem
    private bool isActive;               // Kapýnýn aktif olup olmadýðýný belirten deðiþken
    private float timer;                 // Kapýnýn geçici bir süre etkin olduðunu gösteren zamanlayýcý

    // Kapýnýn baþlangýçta hazýrlandýðý yer
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetInteractableAtGridPosition(gridPosition, this);

        // Kapý baþlangýçta açýk mý yoksa kapalý mý kontrol et
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

        // Kapý etkileþim süre kontrolü
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            isActive = false;
            onInteractionComplete();
        }
    }

    // Kapýyý açmak veya kapatmak için yapýlan etkileþim
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

    // Kapýyý açma iþlemi
    private void OpenDoor()
    {
        isOpen = true;
        animator.SetBool("IsOpen", isOpen);  // Animasyonda kapýnýn açýk olduðunu belirten parametreyi ayarla
        Pathfinding.Instance.SetIsWalkableGridPosition(gridPosition, true);  // Kapýnýn konumunda yürünebilir hale getir

        OnDoorOpened?.Invoke(this, EventArgs.Empty);  // Kapýnýn açýldýðýný bildir
        OnAnyDoorOpened?.Invoke(this, EventArgs.Empty);
    }

    // Kapýyý kapama iþlemi
    private void CloseDoor()
    {
        isOpen = false;
        animator.SetBool("IsOpen", isOpen);  // Animasyonda kapýnýn kapalý olduðunu belirten parametreyi ayarla
        Pathfinding.Instance.SetIsWalkableGridPosition(gridPosition, false);  // Kapýnýn konumunda yürünebilirliði kaldýr
    }
}
