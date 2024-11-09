using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    private Camera mainCamera;
    private RectTransform rectTransform;
    public float scaleProportion = 1f; // Property to adjust the scale proportion
    public float yOffset = 0f; // Property to adjust the Y-axis offset

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera = Camera.main;
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (mainCamera == null || rectTransform == null)
            return;

        // Make the background face the camera
        transform.rotation = Quaternion.LookRotation(mainCamera.transform.forward, mainCamera.transform.up);

        // Adjust the scale of the RectTransform based on the orthographic size of the camera and scale proportion
        float scale = mainCamera.orthographicSize * 2f * scaleProportion;
        rectTransform.localScale = new Vector3(scale * mainCamera.aspect, scale * mainCamera.aspect, 1f);

        // Move the wallpaper on the Y axis relative to the camera with an offset
        rectTransform.position = new Vector3(rectTransform.position.x, mainCamera.transform.position.y + yOffset, rectTransform.position.z);
    }
}