using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    // Birim animasyonlar�n� y�neten sistem

    [SerializeField] private Animator animator;   // Animasyon sistemi bile�eni
    [SerializeField] private Transform bulletProjectilePrefab;   // Mermi prefabs�
    [SerializeField] private Transform shootPointTransform;   // At�� pozisyonu
    [SerializeField] private Transform rifleTransform;   // T�fe�in konumu
    [SerializeField] private Transform swordTransform;   // K�l�c�n konumu

    private void Awake()
    {
        if (TryGetComponent<MoveAction>(out MoveAction moveAction))
        {
            moveAction.OnStartMoving += MoveAction_OnStartMoving;   // Hareket ba�lad�
            moveAction.OnStopMoving += MoveAction_OnStopMoving;   // Hareket durdu
        }

        if (TryGetComponent<ShootAction>(out ShootAction shootAction))
        {
            shootAction.OnShoot += ShootAction_OnShoot;   // At�� yap�ld�
        }

        if (TryGetComponent<SwordAction>(out SwordAction swordAction))
        {
            swordAction.OnSwordActionStarted += SwordAction_OnSwordActionStarted;   // K�l�� eylemi ba�lad�
            swordAction.OnSwordActionCompleted += SwordAction_OnSwordActionCompleted;   // K�l�� eylemi tamamland�
        }
    }

    private void Start()
    {
        EquipRifle();   // Oyuna ba�larken t�fe�i haz�rla
    }

    private void SwordAction_OnSwordActionCompleted(object sender, EventArgs e)
    {
        EquipRifle();   // K�l�� eylemi bitti�inde t�fe�i tekrar haz�rla
    }

    private void SwordAction_OnSwordActionStarted(object sender, EventArgs e)
    {
        EquipSword();   // K�l�� eylemi ba�lad���nda k�l�c� haz�rla
        animator.SetTrigger("SwordSlash");   // K�l�� sald�r�s� animasyonunu tetikle
    }

    private void MoveAction_OnStartMoving(object sender, EventArgs e)
    {
        animator.SetBool("IsWalking", true);   // Y�r�y�� animasyonunu ba�lat
    }

    private void MoveAction_OnStopMoving(object sender, EventArgs e)
    {
        animator.SetBool("IsWalking", false);   // Y�r�y�� animasyonunu durdur
    }

    private void ShootAction_OnShoot(object sender, ShootAction.OnShootEventArgs e)
    {
        animator.SetTrigger("Shoot");   // Ate� etme animasyonunu tetikle

        Transform bulletProjectileTransform =
            Instantiate(bulletProjectilePrefab, shootPointTransform.position, Quaternion.identity);   // Mermiyi ate� pozisyonundan olu�tur

        BulletProjectile bulletProjectile = bulletProjectileTransform.GetComponent<BulletProjectile>();

        Vector3 targetUnitShootAtPosition = e.targetUnit.GetWorldPosition();

        targetUnitShootAtPosition.y = shootPointTransform.position.y;   // At�� yap�ld��� y�kseklik

        bulletProjectile.Setup(targetUnitShootAtPosition);   // Mermiyi hedefe y�nlendir
    }

    private void EquipSword()
    {
        swordTransform.gameObject.SetActive(true);   // K�l�c� aktif et
        rifleTransform.gameObject.SetActive(false);   // T�fe�i pasif et
    }

    private void EquipRifle()
    {
        swordTransform.gameObject.SetActive(false);   // K�l�c� pasif et
        rifleTransform.gameObject.SetActive(true);   // T�fe�i aktif et
    }

}
