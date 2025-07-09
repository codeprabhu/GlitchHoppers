using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [SerializeField] private string nextSceneName = "MainScene";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponent<Collider2D>().enabled = false; // prevent re-trigger

            // Instantly show black screen and message, then load scene
            ScreenFlashManager.Instance.ShowLoadingScreen("Level Complete!\nReturning to main world...", () =>
            {
                SceneManager.LoadScene(nextSceneName);
            });
        }
    }
}
