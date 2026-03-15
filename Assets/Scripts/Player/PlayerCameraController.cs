using System;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [SerializeField] private Vector3 camOffset;
    [SerializeField] private float camRunZoom;
    [SerializeField] private float zoomSpeed = 5f;
    private Camera camera;
    private float camOriginalZoom;
    private PlayerMovementController playerMovementController;

    private void Awake()
    {
        camOriginalZoom = camOffset.y;
        playerMovementController = GetComponent<PlayerMovementController>();
    }

    private void Start()
    {
        //camera = Camera.main;
    }

    void LateUpdate()
    {
        if (camera == null) camera = Camera.main;
        if (camera == null) return;

        CameraFollowPlayer();
        HandlePlayerRunZoomingAndOffset();
    }

    private void CameraFollowPlayer()
    {
        camera.transform.position =
            new Vector3(transform.position.x, transform.position.y, transform.position.z) +
            camOffset; //La camara sigue al jugador en X y Z 

        camera.transform.LookAt(transform.position);
    }

    private void HandlePlayerRunZoomingAndOffset()
    {
        var currentZoom = playerMovementController.IsRunning ? camRunZoom : camOriginalZoom;

        camOffset.y =
            Mathf.Lerp(camOffset.y, currentZoom,
                Time.deltaTime *
                zoomSpeed); //Mueve la camara suavemente entre su posicion actual y la posici�n requerida (currentZoom)
    }
}