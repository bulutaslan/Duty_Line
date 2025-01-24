using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRagdoll : MonoBehaviour
{
    // Birimin ölü bedenini oluþturmak için kullanýlan sistem

    [SerializeField] private Transform ragdollRootBone;   // Ölü bedenin kök kemiði

    // Ragdoll'u kurar
    public void Setup(Transform originalRootBone)
    {
        MatchAllChildTransforms(originalRootBone, ragdollRootBone);   // Orijinal ve ragdoll arasýndaki tüm çocuklarýn pozisyonunu eþleþtirir

        // Ragdoll üzerinde rastgele bir patlama etkisi uygular
        Vector3 randomDir = new Vector3(Random.Range(-1f, +1f), 0, Random.Range(-1f, +1f));
        ApplyExplosionToRagdoll(ragdollRootBone, 300f, transform.position + randomDir, 10f);
    }

    // Çocuk tüm transformlarýn pozisyonlarýný eþler
    private void MatchAllChildTransforms(Transform root, Transform clone)
    {
        foreach (Transform child in root)
        {
            Transform cloneChild = clone.Find(child.name);
            if (cloneChild != null)
            {
                cloneChild.position = child.position;
                cloneChild.rotation = child.rotation;

                MatchAllChildTransforms(child, cloneChild);
            }
        }
    }

    // Patlama etkisini ragdoll üzerine uygular
    private void ApplyExplosionToRagdoll(Transform root, float explosionForce, Vector3 explosionPosition, float explosionRange)
    {
        foreach (Transform child in root)
        {
            if (child.TryGetComponent<Rigidbody>(out Rigidbody childRigidbody))
            {
                childRigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRange);
            }

            ApplyExplosionToRagdoll(child, explosionForce, explosionPosition, explosionRange);
        }
    }
}
