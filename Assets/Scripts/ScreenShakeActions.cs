using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShakeActions : MonoBehaviour
{
    // Ekran sars�nt�lar�n� tetikleyen olaylar� dinleyen s�n�f

    private void Start()
    {
        // �e�itli olaylar�n ekran sars�nt�s�n� tetiklemesi i�in olaylar� dinle
        ShootAction.OnAnyShoot += ShootAction_OnAnyShoot;
        GrenadeProjectile.OnAnyGrenadeExploded += GrenadeProjectile_OnAnyGrenadeExploded;
        SwordAction.OnAnySwordHit += SwordAction_OnAnySwordHit;
    }

    private void SwordAction_OnAnySwordHit(object sender, EventArgs e)
    {
        // K�l�� darbesi an�nda sars�nt�y� tetikle
        ScreenShake.Instance.Shake(2f);   // K���k �iddette sars�nt� olu�turur
    }

    private void GrenadeProjectile_OnAnyGrenadeExploded(object sender, EventArgs e)
    {
        // El bombas� patlad���nda sars�nt�y� tetikle
        ScreenShake.Instance.Shake(5f);   // Daha b�y�k �iddette sars�nt� olu�turur
    }

    private void ShootAction_OnAnyShoot(object sender, ShootAction.OnShootEventArgs e)
    {
        // At�� ger�ekle�ti�inde sars�nt�y� tetikle
        ScreenShake.Instance.Shake();   // Normal �iddette sars�nt� olu�turur
    }
}
