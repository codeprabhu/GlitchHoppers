using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class CutsceneManager : MonoBehaviour
{
    public static CutsceneManager Instance;

    [Header("UI References")]
    public CanvasGroup fadePanel;
    public TextMeshProUGUI subtitleText;

    [Header("Audio")]
    public AudioSource narratorAudioSource;
    public AudioClip narratorClip;

    [Header("Timing")]
    public float fadeDuration = 2f;
    public float textDelay = 1f;
    public string mainMenuSceneName = "MainMenu";

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void PlayFinalBossCutscene()
    {
        StartCoroutine(CutsceneRoutine());
    }

    private IEnumerator CutsceneRoutine()
    {
        // Fade to black
        yield return FadeToBlack();

        yield return new WaitForSeconds(textDelay);

        // Show subtitle
        subtitleText.gameObject.SetActive(true);
        subtitleText.text = "<size=108><b>The Fireknight has fallen.</b>\nHis blazing tyranny reduced to fading embers.\nFrom glitch and flame, a new dawn rises.</size>";

        // Play narration audio
        if (narratorAudioSource != null && narratorClip != null)
        {
            narratorAudioSource.clip = narratorClip;
            narratorAudioSource.Play();

            // Wait for narration to complete
            yield return new WaitWhile(() => narratorAudioSource.isPlaying);
        }
        else
        {
            Debug.LogWarning("Narrator audio not assigned.");
            yield return new WaitForSeconds(3f); // fallback wait time
        }

        // Load Main Menu
        SceneManager.LoadScene(mainMenuSceneName);
    }

    private IEnumerator FadeToBlack()
    {
        fadePanel.gameObject.SetActive(true);
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadePanel.alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
            yield return null;
        }
        fadePanel.alpha = 1f;
    }
}
