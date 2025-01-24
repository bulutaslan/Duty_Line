#define USE_NEW_INPUT_SYSTEM
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    private PlayerInputActions playerInputActions;

    private void Awake()
    {
        // Singleton kontrolü
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);  // Fazlalýk olan InputManager'ý yok et
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);  // Sahneler arasý taþýnabilir hale getir

        try
        {
            playerInputActions = new PlayerInputActions();
            playerInputActions.Player.Enable();
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error initializing PlayerInputActions: " + e.Message);  // PlayerInputActions baþlatýlýrken oluþan hata mesajý
        }
    }

    private void OnDisable()
    {
        if (playerInputActions != null)
        {
            playerInputActions.Player.Disable();  // Input'u devre dýþý býrak
        }
    }

    private void OnDestroy()
    {
        if (playerInputActions != null)
        {
            playerInputActions.Dispose();  // Kaynaklarý temizle
        }
    }

    public Vector2 GetMouseScreenPosition()
    {
#if USE_NEW_INPUT_SYSTEM
        return Mouse.current.position.ReadValue();  // Yeni Input Sistemi ile fare konumunu okur
#else
        return Input.mousePosition;  // Eski Input Sistemi ile fare konumunu okur
#endif
    }

    public bool IsMouseButtonDownThisFrame()
    {
#if USE_NEW_INPUT_SYSTEM
        return playerInputActions.Player.Click.WasPressedThisFrame();  // Yeni Input Sistemi ile fare týklamasýný kontrol eder
#else
        return Input.GetMouseButtonDown(0);  // Eski Input Sistemi ile fare týklamasýný kontrol eder
#endif
    }

    public Vector2 GetCameraMoveVector()
    {
#if USE_NEW_INPUT_SYSTEM
        return playerInputActions.Player.CameraMovement.ReadValue<Vector2>();  // Yeni Input Sistemi ile kamera hareketini okur
#else
        Vector2 inputMoveDir = new Vector2(0, 0);

        if (Input.GetKey(KeyCode.W))
        {
            inputMoveDir.y = +1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputMoveDir.y = -1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputMoveDir.x = -1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputMoveDir.x = +1f;
        }

        return inputMoveDir;
#endif
    }

    public float GetCameraRotateAmount()
    {
#if USE_NEW_INPUT_SYSTEM
        return playerInputActions.Player.CameraRotate.ReadValue<float>();  // Yeni Input Sistemi ile kamera dönüþ miktarýný okur
#else
        float rotateAmount = 0f;

        if (Input.GetKey(KeyCode.Q))
        {
            rotateAmount = +1f;
        }
        if (Input.GetKey(KeyCode.E))
        {
            rotateAmount = -1f;
        }

        return rotateAmount;
#endif
    }

    public float GetCameraZoomAmount()
    {
#if USE_NEW_INPUT_SYSTEM
        return playerInputActions.Player.CameraZoom.ReadValue<float>();  // Yeni Input Sistemi ile kamera zoom miktarýný okur
#else
        float zoomAmount = 0f;

        if (Input.mouseScrollDelta.y > 0)
        {
            zoomAmount = -1f;
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            zoomAmount = +1f;
        }

        return zoomAmount;
#endif
    }
}
