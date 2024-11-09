using UnityEngine;
using System.Collections.Generic;

public class CameraAutoControl : MonoBehaviour
{
    public List<Transform> objectsToTrack; // List of objects to keep in frame
    public float basePadding = 2f; // Base padding around the objects
    public float smoothSpeed = 0.125f; // Speed of the camera movement
    public float baseOrthographicSize = 5f; // Base orthographic size for scaling
    public float xOffset = 0f; // Horizontal offset for the camera

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        if (!mainCamera.orthographic)
        {
            Debug.LogError("Camera is not orthographic!");
        }
    }

    void Update()
    {
        // Check for right and left arrow key inputs
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            xOffset -= 0.1f;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            xOffset += 0.1f;
        }

        // Check for up and down arrow key inputs
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            basePadding += 0.1f;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            basePadding -= 0.1f;
        }
    }

    void LateUpdate()
    {
        if (objectsToTrack.Count == 0)
            return;

        Bounds bounds = new Bounds(objectsToTrack[0].position, Vector3.zero);
        foreach (Transform obj in objectsToTrack)
        {
            bounds.Encapsulate(obj.position);
        }

        Vector3 desiredPosition = bounds.center + new Vector3(xOffset, 0, -10);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        float verticalSize = bounds.size.y * 0.5f;
        float horizontalSize = (bounds.size.x * 0.5f) / mainCamera.aspect;
        float size = Mathf.Max(verticalSize, horizontalSize);

        // Adjust padding relative to the zoom level
        float adjustedPadding = basePadding * (mainCamera.orthographicSize / baseOrthographicSize);
        size += adjustedPadding;

        mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, size, smoothSpeed);

        // Scale objects proportional to the zoom on x and y axes only
        float scaleFactor = mainCamera.orthographicSize / baseOrthographicSize;
        foreach (Transform obj in objectsToTrack)
        {
            obj.localScale = new Vector3(scaleFactor, scaleFactor, obj.localScale.z);
        }
    }
}