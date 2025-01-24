using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRagdollSpawner : MonoBehaviour
{
    // Birimin �ld���nde ragdollunu olu�turan sistem

    [SerializeField] private Transform ragdollPrefab;   // Ragdoll prefabi
    [SerializeField] private Transform originalRootBone;   // Orijinal birimin k�k kemi�i

    private HealthSystem healthSystem;

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();

        healthSystem.OnDead += HealthSystem_OnDead;   // �l�m olay�na abone ol
    }

    // Birim �ld���nde
    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        Transform ragdollTransform = Instantiate(ragdollPrefab, transform.position, transform.rotation);
        UnitRagdoll unitRagdoll = ragdollTransform.GetComponent<UnitRagdoll>();
        unitRagdoll.Setup(originalRootBone);   // �l� bedenin e�le�tirilmesini yap
    }
}
