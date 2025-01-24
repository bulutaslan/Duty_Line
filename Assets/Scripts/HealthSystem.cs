using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    // Bu olay, karakter �ld���nde tetiklenir
    public event EventHandler OnDead;
    public event EventHandler OnDamaged;

    [SerializeField] private int health = 100;  // Karakterin ba�lang�� can miktar�
    private int healthMax;  // Maksimum can miktar�

    private void Awake()
    {
        healthMax = health;  // Maksimum can miktar�n� ayarla
    }

    // Karaktere hasar uygular
    public void Damage(int damageAmount)
    {
        health -= damageAmount;  // Can miktar�n� hasar kadar azalt

        if (health < 0)
        {
            health = 0;  // Can miktar� s�f�r�n alt�na d��erse s�f�r yap
        }

        OnDamaged?.Invoke(this, EventArgs.Empty);  // Hasar ald���nda olay tetiklenir

        if (health == 0)
        {
            Die();  // Can s�f�rsa karakter �l�r
        }
    }

    // Karakterin �l�m�n� ger�ekle�tirir
    private void Die()
    {
        OnDead?.Invoke(this, EventArgs.Empty);  // �l�m olay� tetiklenir
    }

    // Karakterin can�n�n normallesini d�nd�r�r
    public float GetHealthNormalized()
    {
        return (float)health / healthMax;  // Can�n mevcut de�eri ile maksimum de�erinin oran�n� d�nd�r�r
    }
}
