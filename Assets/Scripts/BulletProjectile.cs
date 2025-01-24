using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{

    [SerializeField] private TrailRenderer trailRenderer;  // Kurþun için çizgi efekti
    [SerializeField] private Transform bulletHitVfxPrefab;  // Kurþunun hedefe çarptýðýnda gösterilecek efekt

    private Vector3 targetPosition;  // Hedefin konumu

    public void Setup(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;  // Hedef konumunu ayarla
    }

    private void Update()
    {
        Vector3 moveDir = (targetPosition - transform.position).normalized;  // Hareket yönünü hesapla

        float distanceBeforeMoving = Vector3.Distance(transform.position, targetPosition);  // Hareketten önceki mesafeyi hesapla

        float moveSpeed = 200f;  // Hareket hýzý
        transform.position += moveDir * moveSpeed * Time.deltaTime;  // Hareket ettir

        float distanceAfterMoving = Vector3.Distance(transform.position, targetPosition);  // Hareketten sonraki mesafeyi hesapla

        if (distanceBeforeMoving < distanceAfterMoving)  // Hedefe ulaþýldýysa
        {
            transform.position = targetPosition;  // Konumu hedefe ayarla

            trailRenderer.transform.parent = null;  // Çizgi efekti kaybeder

            Destroy(gameObject);  // Kurþunu yok et

            Instantiate(bulletHitVfxPrefab, targetPosition, Quaternion.identity);  // Hedefe çarptýðýnda efekti göster
        }
    }

}