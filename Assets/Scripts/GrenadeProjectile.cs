using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeProjectile : MonoBehaviour
{
    // Bu olay, herhangi bir el bombas�n�n patlamas� durumunda tetiklenir
    public static event EventHandler OnAnyGrenadeExploded;

    [SerializeField] private Transform grenadeExplodeVfxPrefab;  // Patlama efektinin prefab'�
    [SerializeField] private TrailRenderer trailRenderer;        // El bombas� hareketinin izini �izen trail renderer
    [SerializeField] private AnimationCurve arcYAnimationCurve;  // Y�kseklik e�risi i�in animation curve

    private Vector3 targetPosition;  // Hedef konum
    private Action onGrenadeBehaviourComplete;  // El bombas� davran��� tamamland���nda tetiklenecek i�
    private float totalDistance;  // El bombas� toplam mesafesi
    private Vector3 positionXZ;  // X ve Z eksenindeki pozisyon

    private void Update()
    {
        // Hedef konumdan hareket y�n�n� hesapla
        Vector3 moveDir = (targetPosition - positionXZ).normalized;

        // Hareket h�z�
        float moveSpeed = 15f;
        positionXZ += moveDir * moveSpeed * Time.deltaTime;

        // Mesafeyi hesapla ve normalize et
        float distance = Vector3.Distance(positionXZ, targetPosition);
        float distanceNormalized = 1 - distance / totalDistance;

        // Maksimum y�kseklik
        float maxHeight = totalDistance / 4f;
        float positionY = arcYAnimationCurve.Evaluate(distanceNormalized) * maxHeight;
        transform.position = new Vector3(positionXZ.x, positionY, positionXZ.z);

        // Hedefe yakla��ld���nda i�lem ger�ekle�tirilir
        float reachedTargetDistance = .2f;
        if (Vector3.Distance(positionXZ, targetPosition) < reachedTargetDistance)
        {
            float damageRadius = 4f;  // Hasar yar��ap�
            Collider[] colliderArray = Physics.OverlapSphere(targetPosition, damageRadius);  // �evredeki t�m kollid�rleri al�n

            foreach (Collider collider in colliderArray)
            {
                if (collider.TryGetComponent<Unit>(out Unit targetUnit))
                {
                    targetUnit.Damage(30);  // Hedef bir birimse ona 30 hasar uygula
                }
                if (collider.TryGetComponent<DestructibleCrate>(out DestructibleCrate destructibleCrate))
                {
                    destructibleCrate.Damage();  // Hedef yok edilebilir bir kutuysa yok et
                }
            }

            OnAnyGrenadeExploded?.Invoke(this, EventArgs.Empty);  // T�m el bombalar�n�n patlamas� olay�n� tetikle

            trailRenderer.transform.parent = null;  // Trail efekti nesnesini ana objeden kopart

            Instantiate(grenadeExplodeVfxPrefab, targetPosition + Vector3.up * 1f, Quaternion.identity);  // Patlama efektini olu�tur

            Destroy(gameObject);  // El bombas� objesini yok et

            onGrenadeBehaviourComplete();  // Davran�� tamamland� i�lemini tetikle
        }
    }

    // El bombas� hedef konumunu ve davran�� tamamland���nda yap�lacak i�lemi ayarla
    public void Setup(GridPosition targetGridPosition, Action onGrenadeBehaviourComplete)
    {
        this.onGrenadeBehaviourComplete = onGrenadeBehaviourComplete;
        targetPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);

        positionXZ = transform.position;
        positionXZ.y = 0;
        totalDistance = Vector3.Distance(positionXZ, targetPosition);
    }
}
