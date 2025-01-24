using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleCrate : MonoBehaviour
{
    // T�m kutular�n yok oldu�u olay�n� dinlemek i�in tan�mlanan olay
    public static event EventHandler OnAnyDestroyed;

    [SerializeField] private Transform crateDestroyedPrefab;

    private GridPosition gridPosition;

    // Oyunun ba��nda kutunun konumunu al
    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
    }

    // Kutunun a�daki konumunu d�nd�r
    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

    // Kutuyu yok etme i�lemi
    public void Damage()
    {
        // Y�k�lm�� kutu g�r�n�m�n� olu�tur
        Transform crateDestroyedTransform = Instantiate(crateDestroyedPrefab, transform.position, transform.rotation);

        // T�m �ocuk par�alar� �zerinde patlama etkisi uygula
        ApplyExplosionToChildren(crateDestroyedTransform, 150f, transform.position, 10f);

        // Kutuyu yok et
        Destroy(gameObject);

        // Herhangi bir kutunun yok olma olay�n� tetikle
        OnAnyDestroyed?.Invoke(this, EventArgs.Empty);
    }

    // �ocuklar �zerinde patlama etkisi uygulayan yard�mc� metod
    private void ApplyExplosionToChildren(Transform root, float explosionForce, Vector3 explosionPosition, float explosionRange)
    {
        foreach (Transform child in root)
        {
            if (child.TryGetComponent<Rigidbody>(out Rigidbody childRigidbody))
            {
                childRigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRange);
            }

            // Rek�rsif olarak bu i�levi �ocuklar�na uygula
            ApplyExplosionToChildren(child, explosionForce, explosionPosition, explosionRange);
        }
    }
}
