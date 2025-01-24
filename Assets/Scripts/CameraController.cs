using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{

    private const float MIN_FOLLOW_Y_OFFSET = 2f;  // Kamera takip offsetinin minimum deðeri
    private const float MAX_FOLLOW_Y_OFFSET = 12f;  // Kamera takip offsetinin maksimum deðeri

    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;  // Cinemachine sanal kamerasý

    private CinemachineTransposer cinemachineTransposer;  // Cinemachine transposer komponenti
    private Vector3 targetFollowOffset;  // Kameranýn takip ettiði offset

    private void Start()
    {
        cinemachineTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();  // Transposer komponentini al
        targetFollowOffset = cinemachineTransposer.m_FollowOffset;  // Takip offsetini al
    }

    private void Update()
    {
        HandleMovement();  // Hareketi iþle
        HandleRotation();  // Dönmeyi iþle
        HandleZoom();  // Zoom iþle
    }

    private void HandleMovement()
    {
        Vector2 inputMoveDir = InputManager.Instance.GetCameraMoveVector();  // Kameranýn hareket yönünü al

        float moveSpeed = 10f;  // Hareket hýzý

        Vector3 moveVector = transform.forward * inputMoveDir.y + transform.right * inputMoveDir.x;  // Hareket vektörü
        transform.position += moveVector * moveSpeed * Time.deltaTime;  // Kamerayý hareket ettir
    }

    private void HandleRotation()
    {
        Vector3 rotationVector = new Vector3(0, 0, 0);  // Dönme vektörü

        rotationVector.y = InputManager.Instance.GetCameraRotateAmount();  // Dönme miktarýný al

        float rotationSpeed = 100f;  // Dönme hýzý
        transform.eulerAngles += rotationVector * rotationSpeed * Time.deltaTime;  // Kamerayý döndür
    }

    private void HandleZoom()
    {
        float zoomIncreaseAmount = 1f;  // Zoom arttýrma miktarý
        targetFollowOffset.y += InputManager.Instance.GetCameraZoomAmount() * zoomIncreaseAmount;  // Takip offsetini ayarla

        targetFollowOffset.y = Mathf.Clamp(targetFollowOffset.y, MIN_FOLLOW_Y_OFFSET, MAX_FOLLOW_Y_OFFSET);  // Takip offsetini sýnýrla

        float zoomSpeed = 5f;  // Zoom hýzýný ayarla
        cinemachineTransposer.m_FollowOffset =  // Cinemachine transposerýn takip offsetini ayarla
            Vector3.Lerp(cinemachineTransposer.m_FollowOffset, targetFollowOffset, Time.deltaTime * zoomSpeed);  // Takip offsetini yumuþak bir geçiþle güncelle
    }

}