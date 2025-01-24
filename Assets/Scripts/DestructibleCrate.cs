using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleCrate : MonoBehaviour
{
    // Tüm kutularýn yok olduðu olayýný dinlemek için tanýmlanan olay
    public static event EventHandler OnAnyDestroyed;

    [SerializeField] private Transform crateDestroyedPrefab;

    private GridPosition gridPosition;

    // Oyunun baþýnda kutunun konumunu al
    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
    }

    // Kutunun aðdaki konumunu döndür
    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

    // Kutuyu yok etme iþlemi
    public void Damage()
    {
        // Yýkýlmýþ kutu görünümünü oluþtur
        Transform crateDestroyedTransform = Instantiate(crateDestroyedPrefab, transform.position, transform.rotation);

        // Tüm çocuk parçalarý üzerinde patlama etkisi uygula
        ApplyExplosionToChildren(crateDestroyedTransform, 150f, transform.position, 10f);

        // Kutuyu yok et
        Destroy(gameObject);

        // Herhangi bir kutunun yok olma olayýný tetikle
        OnAnyDestroyed?.Invoke(this, EventArgs.Empty);
    }

    // Çocuklar üzerinde patlama etkisi uygulayan yardýmcý metod
    private void ApplyExplosionToChildren(Transform root, float explosionForce, Vector3 explosionPosition, float explosionRange)
    {
        foreach (Transform child in root)
        {
            if (child.TryGetComponent<Rigidbody>(out Rigidbody childRigidbody))
            {
                childRigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRange);
            }

            // Rekürsif olarak bu iþlevi çocuklarýna uygula
            ApplyExplosionToChildren(child, explosionForce, explosionPosition, explosionRange);
        }
    }
}
