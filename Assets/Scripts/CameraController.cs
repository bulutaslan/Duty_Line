using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{

    private const float MIN_FOLLOW_Y_OFFSET = 2f;  // Kamera takip offsetinin minimum de�eri
    private const float MAX_FOLLOW_Y_OFFSET = 12f;  // Kamera takip offsetinin maksimum de�eri

    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;  // Cinemachine sanal kameras�

    private CinemachineTransposer cinemachineTransposer;  // Cinemachine transposer komponenti
    private Vector3 targetFollowOffset;  // Kameran�n takip etti�i offset

    private void Start()
    {
        cinemachineTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();  // Transposer komponentini al
        targetFollowOffset = cinemachineTransposer.m_FollowOffset;  // Takip offsetini al
    }

    private void Update()
    {
        HandleMovement();  // Hareketi i�le
        HandleRotation();  // D�nmeyi i�le
        HandleZoom();  // Zoom i�le
    }

    private void HandleMovement()
    {
        Vector2 inputMoveDir = InputManager.Instance.GetCameraMoveVector();  // Kameran�n hareket y�n�n� al

        float moveSpeed = 10f;  // Hareket h�z�

        Vector3 moveVector = transform.forward * inputMoveDir.y + transform.right * inputMoveDir.x;  // Hareket vekt�r�
        transform.position += moveVector * moveSpeed * Time.deltaTime;  // Kameray� hareket ettir
    }

    private void HandleRotation()
    {
        Vector3 rotationVector = new Vector3(0, 0, 0);  // D�nme vekt�r�

        rotationVector.y = InputManager.Instance.GetCameraRotateAmount();  // D�nme miktar�n� al

        float rotationSpeed = 100f;  // D�nme h�z�
        transform.eulerAngles += rotationVector * rotationSpeed * Time.deltaTime;  // Kameray� d�nd�r
    }

    private void HandleZoom()
    {
        float zoomIncreaseAmount = 1f;  // Zoom artt�rma miktar�
        targetFollowOffset.y += InputManager.Instance.GetCameraZoomAmount() * zoomIncreaseAmount;  // Takip offsetini ayarla

        targetFollowOffset.y = Mathf.Clamp(targetFollowOffset.y, MIN_FOLLOW_Y_OFFSET, MAX_FOLLOW_Y_OFFSET);  // Takip offsetini s�n�rla

        float zoomSpeed = 5f;  // Zoom h�z�n� ayarla
        cinemachineTransposer.m_FollowOffset =  // Cinemachine transposer�n takip offsetini ayarla
            Vector3.Lerp(cinemachineTransposer.m_FollowOffset, targetFollowOffset, Time.deltaTime * zoomSpeed);  // Takip offsetini yumu�ak bir ge�i�le g�ncelle
    }

}