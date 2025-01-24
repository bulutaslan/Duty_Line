using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField] private bool invert;  // Kamera y�n�n� tersine �evirmek isteyip istemedi�imizi belirten bool de�i�ken

    private Transform cameraTransform;  // Kamera konumunu tutan transform

    private void Awake()
    {
        cameraTransform = Camera.main.transform;  // Ana kameran�n pozisyonunu al
    }

    private void LateUpdate()
    {
        if (invert)
        {
            Vector3 dirToCamera = (cameraTransform.position - transform.position).normalized;  // Kamera y�n� do�rultusunu al
            transform.LookAt(transform.position + dirToCamera * -1);  // Tersine �evir
        }
        else
        {
            transform.LookAt(cameraTransform);  // Kamera y�n�ne do�ru bak
        }
    }
}
