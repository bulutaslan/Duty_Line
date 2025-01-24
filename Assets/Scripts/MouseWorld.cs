using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseWorld : MonoBehaviour
{
    private static MouseWorld instance;  // Static �rne�i

    [SerializeField] private LayerMask mousePlaneLayerMask;  // Fare t�klamas�n�n ger�ekle�ti�i d�zlem maskesi

    private void Awake()
    {
        instance = this;
    }

    // D�nyadaki mouse t�klamas�n�n konumunu al�r
    public static Vector3 GetPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(InputManager.Instance.GetMouseScreenPosition());
        Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, instance.mousePlaneLayerMask);  // D�nen hiti al
        return raycastHit.point;  // D�nen konum
    }
}
