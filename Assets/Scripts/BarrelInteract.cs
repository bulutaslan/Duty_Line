using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelInteract : MonoBehaviour, IInteractable
{

    [SerializeField] private Transform barrelDestroyedPrefab;  // Baril yok olduðunda kullanýlacak prefab
    [SerializeField] private GameObject visualGameObject;  // Görsel obje

    private GridPosition gridPosition;  // Barilin konumunu temsil eden deðiþken
    private Action onInteractionComplete;  // Etkileþim tamamlandýðýnda yapýlacak iþlem
    private bool isActive;  // Etkileþimin aktif olup olmadýðýný kontrol eden deðiþken
    private float timer;  // Etkileþim süresini takip eden zamanlayýcý

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);  // Barilin grid üzerindeki konumunu belirle
        LevelGrid.Instance.SetInteractableAtGridPosition(gridPosition, this);  // Grid üzerindeki etkileþim alanýný belirle
        Pathfinding.Instance.SetIsWalkableGridPosition(gridPosition, false);  // Grid üzerindeki yürüme yeteneðini kaldýr
    }

    private void Update()
    {
        if (!isActive)
        {
            return;  // Eðer etkileþim aktif deðilse çýk
        }

        timer -= Time.deltaTime;  // Zamanlayýcýyý azalt

        if (timer <= 0f)
        {
            isActive = false;

            LevelGrid.Instance.ClearInteractableAtGridPosition(gridPosition);  // Grid üzerindeki etkileþim alanýný temizle
            Pathfinding.Instance.SetIsWalkableGridPosition(gridPosition, true);  // Grid üzerindeki yürüme yeteneðini geri getir

            Destroy(gameObject);  // Barili yok et
            onInteractionComplete();  // Etkileþim tamamlandý iþlemini yap
        }
    }

    public void Interact(Action onInteractionComplete)
    {
        this.onInteractionComplete = onInteractionComplete;
        isActive = true;
        timer = 0.5f;

        visualGameObject.SetActive(false);  // Görsel objeyi devre dýþý býrak

        Transform barrelDestroyedTransform = Instantiate(barrelDestroyedPrefab, transform.position, transform.rotation);  // Baril yok olduðunda kullanýlacak efekt
        ApplyExplosionToChildren(barrelDestroyedTransform, 250f, transform.position, 10f);  // Çocuklarýna patlama etkisi uygula
    }

    private void ApplyExplosionToChildren(Transform root, float explosionForce, Vector3 explosionPosition, float explosionRange)
    {
        foreach (Transform child in root)
        {
            if (child.TryGetComponent<Rigidbody>(out Rigidbody childRigidbody))
            {
                childRigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRange);  // Çocuða patlama kuvveti uygula
            }

            ApplyExplosionToChildren(child, explosionForce, explosionPosition, explosionRange);  // Çocuklarýn çocuklarýna da patlama kuvveti uygula
        }
    }

}