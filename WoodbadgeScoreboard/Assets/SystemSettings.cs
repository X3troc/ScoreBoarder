using UnityEngine;

public class SystemSettings : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Set the target frame rate to 100
        Application.targetFrameRate = 100;
    }

    // Update is called once per frame
    void Update()
    {
        // Exit the game when the ESC key is hit
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}