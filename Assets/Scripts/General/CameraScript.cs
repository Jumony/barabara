using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    public GameObject player;
    public Vector3 offset;
    [Header("Smooth Speeds")]
    public float zoomSmoothSpeed = 0.125f;
    public float followSmoothSpeed = 0.125f;
    public float zoomFactor = 2f;
    public float zoomSpeed = 10f;
    [Tooltip("Max distance the camera can go from the player")]
    public float maxDistance = 5f;
    public float mouseInfluenceFactor = 0.25f;

    private float defaultSize;
    private Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
        defaultSize = cam.orthographicSize;
    }
    void LateUpdate()
    {
        Vector3 defaultPosition = player.transform.position + offset;
        Vector3 mouseWorldPosition = cam.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0f;

        Vector3 mouseInfluence = (mouseWorldPosition - player.transform.position) * mouseInfluenceFactor;

        if (mouseInfluence.magnitude > maxDistance)
        {
            mouseInfluence = mouseInfluence.normalized * Mathf.Clamp(mouseInfluence.magnitude, 0, maxDistance);
        }
        defaultPosition += mouseInfluence;

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, defaultPosition, followSmoothSpeed);
        transform.position = smoothedPosition;
    }

    public void CameraZoom()
    {
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, defaultSize / zoomFactor, Time.deltaTime * zoomSpeed);
    }

    public void ResetCameraZoom()
    {
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, defaultSize, Time.deltaTime * zoomSpeed);
    }
}
