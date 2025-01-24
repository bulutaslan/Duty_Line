using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField] private bool invert;  // Kamera yönünü tersine çevirmek isteyip istemediðimizi belirten bool deðiþken

    private Transform cameraTransform;  // Kamera konumunu tutan transform

    private void Awake()
    {
        cameraTransform = Camera.main.transform;  // Ana kameranýn pozisyonunu al
    }

    private void LateUpdate()
    {
        if (invert)
        {
            Vector3 dirToCamera = (cameraTransform.position - transform.position).normalized;  // Kamera yönü doðrultusunu al
            transform.LookAt(transform.position + dirToCamera * -1);  // Tersine çevir
        }
        else
        {
            transform.LookAt(cameraTransform);  // Kamera yönüne doðru bak
        }
    }
}
