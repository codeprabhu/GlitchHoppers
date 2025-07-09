using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerFallCheck : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float fallDistance = 15f;
    [SerializeField] private GameObject deathCanvas;

    private bool isDead = false;

    void Update()
    {
        if (!isDead && transform.position.y < cameraTransform.position.y - fallDistance)
        {
            isDead = true;
            deathCanvas.SetActive(true);
            Time.timeScale = 0f; // Pause the game
        }
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Glitch"); // Change to your menu scene name
    }
}
