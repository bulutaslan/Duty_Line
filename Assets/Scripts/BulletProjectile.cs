using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{

    [SerializeField] private TrailRenderer trailRenderer;  // Kur�un i�in �izgi efekti
    [SerializeField] private Transform bulletHitVfxPrefab;  // Kur�unun hedefe �arpt���nda g�sterilecek efekt

    private Vector3 targetPosition;  // Hedefin konumu

    public void Setup(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;  // Hedef konumunu ayarla
    }

    private void Update()
    {
        Vector3 moveDir = (targetPosition - transform.position).normalized;  // Hareket y�n�n� hesapla

        float distanceBeforeMoving = Vector3.Distance(transform.position, targetPosition);  // Hareketten �nceki mesafeyi hesapla

        float moveSpeed = 200f;  // Hareket h�z�
        transform.position += moveDir * moveSpeed * Time.deltaTime;  // Hareket ettir

        float distanceAfterMoving = Vector3.Distance(transform.position, targetPosition);  // Hareketten sonraki mesafeyi hesapla

        if (distanceBeforeMoving < distanceAfterMoving)  // Hedefe ula��ld�ysa
        {
            transform.position = targetPosition;  // Konumu hedefe ayarla

            trailRenderer.transform.parent = null;  // �izgi efekti kaybeder

            Destroy(gameObject);  // Kur�unu yok et

            Instantiate(bulletHitVfxPrefab, targetPosition, Quaternion.identity);  // Hedefe �arpt���nda efekti g�ster
        }
    }

}