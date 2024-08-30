using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ZoomController : MonoBehaviour
{
    public Camera mainCamera;
    public float cameraSize = 10f;
    public float zoomOutSize = 30f;
    public bool isZoomedOut = false;
    private bool isButtonHeld = false;

    void Start()
    {
        if (mainCamera == null)
        {
            Debug.LogError("Main Camera is not assigned in the inspector!");
        }
        else
        {
            mainCamera.orthographicSize = cameraSize;
        }
    }

    void Update()
    {
        HandleZoom();
    }

    void HandleZoom()
    {
        if (mainCamera != null)
        {
            if (isButtonHeld) // Змінено: зум залежить від утримання кнопки
            {
                mainCamera.orthographicSize = zoomOutSize;
            }
            else
            {
                mainCamera.orthographicSize = cameraSize;
            }
        }
    }

    public void OnPointerDown()
    {
        isButtonHeld = true; // Встановлюємо прапорець в true, коли кнопка утримується
    }

    public void OnPointerUp()
    {
        isButtonHeld = false; // Встановлюємо прапорець в false, коли кнопка відпускається
    }

    public void ZoomOut()
    {
        isZoomedOut = !isZoomedOut; // Змінюємо стан збільшення
    }
}
