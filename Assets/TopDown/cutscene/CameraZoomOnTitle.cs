using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraZoomOnTitle : MonoBehaviour
{
    public Transform focusTarget;   // Assign to TitleFocusTarget
    public float zoomSpeed = 1f;
    public float zoomThreshold = 1.5f;
    public float delayBeforeZoom = 1f;
    public string nextSceneName = "MainMenu";

    private float timer = 0f;
    private bool zooming = false;

    void Update()
    {
        if (!zooming)
        {
            timer += Time.deltaTime;
            if (timer >= delayBeforeZoom)
                zooming = true;
            return;
        }

        // Smooth move toward title
        transform.position = Vector3.Lerp(transform.position, focusTarget.position, Time.deltaTime * zoomSpeed);

        // Optional: zoom in (move closer)
        float distance = Vector3.Distance(transform.position, focusTarget.position);
        if (distance <= zoomThreshold)
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
