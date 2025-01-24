using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShakeActions : MonoBehaviour
{
    // Ekran sarsýntýlarýný tetikleyen olaylarý dinleyen sýnýf

    private void Start()
    {
        // Çeþitli olaylarýn ekran sarsýntýsýný tetiklemesi için olaylarý dinle
        ShootAction.OnAnyShoot += ShootAction_OnAnyShoot;
        GrenadeProjectile.OnAnyGrenadeExploded += GrenadeProjectile_OnAnyGrenadeExploded;
        SwordAction.OnAnySwordHit += SwordAction_OnAnySwordHit;
    }

    private void SwordAction_OnAnySwordHit(object sender, EventArgs e)
    {
        // Kýlýç darbesi anýnda sarsýntýyý tetikle
        ScreenShake.Instance.Shake(2f);   // Küçük þiddette sarsýntý oluþturur
    }

    private void GrenadeProjectile_OnAnyGrenadeExploded(object sender, EventArgs e)
    {
        // El bombasý patladýðýnda sarsýntýyý tetikle
        ScreenShake.Instance.Shake(5f);   // Daha büyük þiddette sarsýntý oluþturur
    }

    private void ShootAction_OnAnyShoot(object sender, ShootAction.OnShootEventArgs e)
    {
        // Atýþ gerçekleþtiðinde sarsýntýyý tetikle
        ScreenShake.Instance.Shake();   // Normal þiddette sarsýntý oluþturur
    }
}
