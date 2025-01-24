using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelInteract : MonoBehaviour, IInteractable
{

    [SerializeField] private Transform barrelDestroyedPrefab;  // Baril yok oldu�unda kullan�lacak prefab
    [SerializeField] private GameObject visualGameObject;  // G�rsel obje

    private GridPosition gridPosition;  // Barilin konumunu temsil eden de�i�ken
    private Action onInteractionComplete;  // Etkile�im tamamland���nda yap�lacak i�lem
    private bool isActive;  // Etkile�imin aktif olup olmad���n� kontrol eden de�i�ken
    private float timer;  // Etkile�im s�resini takip eden zamanlay�c�

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);  // Barilin grid �zerindeki konumunu belirle
        LevelGrid.Instance.SetInteractableAtGridPosition(gridPosition, this);  // Grid �zerindeki etkile�im alan�n� belirle
        Pathfinding.Instance.SetIsWalkableGridPosition(gridPosition, false);  // Grid �zerindeki y�r�me yetene�ini kald�r
    }

    private void Update()
    {
        if (!isActive)
        {
            return;  // E�er etkile�im aktif de�ilse ��k
        }

        timer -= Time.deltaTime;  // Zamanlay�c�y� azalt

        if (timer <= 0f)
        {
            isActive = false;

            LevelGrid.Instance.ClearInteractableAtGridPosition(gridPosition);  // Grid �zerindeki etkile�im alan�n� temizle
            Pathfinding.Instance.SetIsWalkableGridPosition(gridPosition, true);  // Grid �zerindeki y�r�me yetene�ini geri getir

            Destroy(gameObject);  // Barili yok et
            onInteractionComplete();  // Etkile�im tamamland� i�lemini yap
        }
    }

    public void Interact(Action onInteractionComplete)
    {
        this.onInteractionComplete = onInteractionComplete;
        isActive = true;
        timer = 0.5f;

        visualGameObject.SetActive(false);  // G�rsel objeyi devre d��� b�rak

        Transform barrelDestroyedTransform = Instantiate(barrelDestroyedPrefab, transform.position, transform.rotation);  // Baril yok oldu�unda kullan�lacak efekt
        ApplyExplosionToChildren(barrelDestroyedTransform, 250f, transform.position, 10f);  // �ocuklar�na patlama etkisi uygula
    }

    private void ApplyExplosionToChildren(Transform root, float explosionForce, Vector3 explosionPosition, float explosionRange)
    {
        foreach (Transform child in root)
        {
            if (child.TryGetComponent<Rigidbody>(out Rigidbody childRigidbody))
            {
                childRigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRange);  // �ocu�a patlama kuvveti uygula
            }

            ApplyExplosionToChildren(child, explosionForce, explosionPosition, explosionRange);  // �ocuklar�n �ocuklar�na da patlama kuvveti uygula
        }
    }

}