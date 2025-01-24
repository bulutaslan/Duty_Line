using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeProjectile : MonoBehaviour
{
    // Bu olay, herhangi bir el bombasýnýn patlamasý durumunda tetiklenir
    public static event EventHandler OnAnyGrenadeExploded;

    [SerializeField] private Transform grenadeExplodeVfxPrefab;  // Patlama efektinin prefab'ý
    [SerializeField] private TrailRenderer trailRenderer;        // El bombasý hareketinin izini çizen trail renderer
    [SerializeField] private AnimationCurve arcYAnimationCurve;  // Yükseklik eðrisi için animation curve

    private Vector3 targetPosition;  // Hedef konum
    private Action onGrenadeBehaviourComplete;  // El bombasý davranýþý tamamlandýðýnda tetiklenecek iþ
    private float totalDistance;  // El bombasý toplam mesafesi
    private Vector3 positionXZ;  // X ve Z eksenindeki pozisyon

    private void Update()
    {
        // Hedef konumdan hareket yönünü hesapla
        Vector3 moveDir = (targetPosition - positionXZ).normalized;

        // Hareket hýzý
        float moveSpeed = 15f;
        positionXZ += moveDir * moveSpeed * Time.deltaTime;

        // Mesafeyi hesapla ve normalize et
        float distance = Vector3.Distance(positionXZ, targetPosition);
        float distanceNormalized = 1 - distance / totalDistance;

        // Maksimum yükseklik
        float maxHeight = totalDistance / 4f;
        float positionY = arcYAnimationCurve.Evaluate(distanceNormalized) * maxHeight;
        transform.position = new Vector3(positionXZ.x, positionY, positionXZ.z);

        // Hedefe yaklaþýldýðýnda iþlem gerçekleþtirilir
        float reachedTargetDistance = .2f;
        if (Vector3.Distance(positionXZ, targetPosition) < reachedTargetDistance)
        {
            float damageRadius = 4f;  // Hasar yarýçapý
            Collider[] colliderArray = Physics.OverlapSphere(targetPosition, damageRadius);  // Çevredeki tüm kollidörleri alýn

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

            OnAnyGrenadeExploded?.Invoke(this, EventArgs.Empty);  // Tüm el bombalarýnýn patlamasý olayýný tetikle

            trailRenderer.transform.parent = null;  // Trail efekti nesnesini ana objeden kopart

            Instantiate(grenadeExplodeVfxPrefab, targetPosition + Vector3.up * 1f, Quaternion.identity);  // Patlama efektini oluþtur

            Destroy(gameObject);  // El bombasý objesini yok et

            onGrenadeBehaviourComplete();  // Davranýþ tamamlandý iþlemini tetikle
        }
    }

    // El bombasý hedef konumunu ve davranýþ tamamlandýðýnda yapýlacak iþlemi ayarla
    public void Setup(GridPosition targetGridPosition, Action onGrenadeBehaviourComplete)
    {
        this.onGrenadeBehaviourComplete = onGrenadeBehaviourComplete;
        targetPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);

        positionXZ = transform.position;
        positionXZ.y = 0;
        totalDistance = Vector3.Distance(positionXZ, targetPosition);
    }
}
