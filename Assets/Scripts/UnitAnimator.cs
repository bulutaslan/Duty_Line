using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    // Birim animasyonlarýný yöneten sistem

    [SerializeField] private Animator animator;   // Animasyon sistemi bileþeni
    [SerializeField] private Transform bulletProjectilePrefab;   // Mermi prefabsý
    [SerializeField] private Transform shootPointTransform;   // Atýþ pozisyonu
    [SerializeField] private Transform rifleTransform;   // Tüfeðin konumu
    [SerializeField] private Transform swordTransform;   // Kýlýcýn konumu

    private void Awake()
    {
        if (TryGetComponent<MoveAction>(out MoveAction moveAction))
        {
            moveAction.OnStartMoving += MoveAction_OnStartMoving;   // Hareket baþladý
            moveAction.OnStopMoving += MoveAction_OnStopMoving;   // Hareket durdu
        }

        if (TryGetComponent<ShootAction>(out ShootAction shootAction))
        {
            shootAction.OnShoot += ShootAction_OnShoot;   // Atýþ yapýldý
        }

        if (TryGetComponent<SwordAction>(out SwordAction swordAction))
        {
            swordAction.OnSwordActionStarted += SwordAction_OnSwordActionStarted;   // Kýlýç eylemi baþladý
            swordAction.OnSwordActionCompleted += SwordAction_OnSwordActionCompleted;   // Kýlýç eylemi tamamlandý
        }
    }

    private void Start()
    {
        EquipRifle();   // Oyuna baþlarken tüfeði hazýrla
    }

    private void SwordAction_OnSwordActionCompleted(object sender, EventArgs e)
    {
        EquipRifle();   // Kýlýç eylemi bittiðinde tüfeði tekrar hazýrla
    }

    private void SwordAction_OnSwordActionStarted(object sender, EventArgs e)
    {
        EquipSword();   // Kýlýç eylemi baþladýðýnda kýlýcý hazýrla
        animator.SetTrigger("SwordSlash");   // Kýlýç saldýrýsý animasyonunu tetikle
    }

    private void MoveAction_OnStartMoving(object sender, EventArgs e)
    {
        animator.SetBool("IsWalking", true);   // Yürüyüþ animasyonunu baþlat
    }

    private void MoveAction_OnStopMoving(object sender, EventArgs e)
    {
        animator.SetBool("IsWalking", false);   // Yürüyüþ animasyonunu durdur
    }

    private void ShootAction_OnShoot(object sender, ShootAction.OnShootEventArgs e)
    {
        animator.SetTrigger("Shoot");   // Ateþ etme animasyonunu tetikle

        Transform bulletProjectileTransform =
            Instantiate(bulletProjectilePrefab, shootPointTransform.position, Quaternion.identity);   // Mermiyi ateþ pozisyonundan oluþtur

        BulletProjectile bulletProjectile = bulletProjectileTransform.GetComponent<BulletProjectile>();

        Vector3 targetUnitShootAtPosition = e.targetUnit.GetWorldPosition();

        targetUnitShootAtPosition.y = shootPointTransform.position.y;   // Atýþ yapýldýðý yükseklik

        bulletProjectile.Setup(targetUnitShootAtPosition);   // Mermiyi hedefe yönlendir
    }

    private void EquipSword()
    {
        swordTransform.gameObject.SetActive(true);   // Kýlýcý aktif et
        rifleTransform.gameObject.SetActive(false);   // Tüfeði pasif et
    }

    private void EquipRifle()
    {
        swordTransform.gameObject.SetActive(false);   // Kýlýcý pasif et
        rifleTransform.gameObject.SetActive(true);   // Tüfeði aktif et
    }

}
