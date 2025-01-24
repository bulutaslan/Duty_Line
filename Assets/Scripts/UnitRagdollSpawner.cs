using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRagdollSpawner : MonoBehaviour
{
    // Birimin öldüðünde ragdollunu oluþturan sistem

    [SerializeField] private Transform ragdollPrefab;   // Ragdoll prefabi
    [SerializeField] private Transform originalRootBone;   // Orijinal birimin kök kemiði

    private HealthSystem healthSystem;

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();

        healthSystem.OnDead += HealthSystem_OnDead;   // Ölüm olayýna abone ol
    }

    // Birim öldüðünde
    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        Transform ragdollTransform = Instantiate(ragdollPrefab, transform.position, transform.rotation);
        UnitRagdoll unitRagdoll = ragdollTransform.GetComponent<UnitRagdoll>();
        unitRagdoll.Setup(originalRootBone);   // Ölü bedenin eþleþtirilmesini yap
    }
}
