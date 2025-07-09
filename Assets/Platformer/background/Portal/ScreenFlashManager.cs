using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class ScreenFlashManager : MonoBehaviour
{
    public CanvasGroup flashGroup;         // Black screen
    public CanvasGroup messageGroup;       // Message area
    public TextMeshProUGUI messageText;    // Message text

    public static ScreenFlashManager Instance;

    private void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional if you want it to persist across scenes
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Start hidden
        flashGroup.alpha = 0;
        messageGroup.alpha = 0;
    }

    /// <summary>
    /// Instantly show black screen + message, then invoke the action (like loading a scene).
    /// </summary>
    public void ShowLoadingScreen(string message, System.Action onComplete)
    {
        StartCoroutine(DoLoadingSequence(message, onComplete));
    }

    private IEnumerator DoLoadingSequence(string message, System.Action onComplete)
    {
        // Immediately show full black screen and message
        flashGroup.alpha = 1f;
        messageText.text = message;
        messageGroup.alpha = 1f;

        // Optional tiny pause before loading, if needed
        yield return new WaitForSeconds(1.75f);

        // Call the scene loader or any other logic
        onComplete?.Invoke();

        // Do NOT fade out — let the next scene handle visuals
    }
}
