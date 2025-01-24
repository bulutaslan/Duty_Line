using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    // Bu olay, karakter öldüðünde tetiklenir
    public event EventHandler OnDead;
    public event EventHandler OnDamaged;

    [SerializeField] private int health = 100;  // Karakterin baþlangýç can miktarý
    private int healthMax;  // Maksimum can miktarý

    private void Awake()
    {
        healthMax = health;  // Maksimum can miktarýný ayarla
    }

    // Karaktere hasar uygular
    public void Damage(int damageAmount)
    {
        health -= damageAmount;  // Can miktarýný hasar kadar azalt

        if (health < 0)
        {
            health = 0;  // Can miktarý sýfýrýn altýna düþerse sýfýr yap
        }

        OnDamaged?.Invoke(this, EventArgs.Empty);  // Hasar aldýðýnda olay tetiklenir

        if (health == 0)
        {
            Die();  // Can sýfýrsa karakter ölür
        }
    }

    // Karakterin ölümünü gerçekleþtirir
    private void Die()
    {
        OnDead?.Invoke(this, EventArgs.Empty);  // Ölüm olayý tetiklenir
    }

    // Karakterin canýnýn normallesini döndürür
    public float GetHealthNormalized()
    {
        return (float)health / healthMax;  // Canýn mevcut deðeri ile maksimum deðerinin oranýný döndürür
    }
}
