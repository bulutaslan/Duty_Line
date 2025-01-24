using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseWorld : MonoBehaviour
{
    private static MouseWorld instance;  // Static örneði

    [SerializeField] private LayerMask mousePlaneLayerMask;  // Fare týklamasýnýn gerçekleþtiði düzlem maskesi

    private void Awake()
    {
        instance = this;
    }

    // Dünyadaki mouse týklamasýnýn konumunu alýr
    public static Vector3 GetPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(InputManager.Instance.GetMouseScreenPosition());
        Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, instance.mousePlaneLayerMask);  // Dönen hiti al
        return raycastHit.point;  // Dönen konum
    }
}
