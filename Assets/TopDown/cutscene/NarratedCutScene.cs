using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class NarratedCutscene : MonoBehaviour
{
    // New cinematic reveal images & texts
    public Image studioBackgroundImage;
    public TextMeshProUGUI studioText;
    public Image gameTitleBackgroundImage;
    public TextMeshProUGUI gameTitleText;

    // Optional settings
    public float textFadeDuration = 1f;
    public float glitchFlashDuration = 0.2f;

    public Image fadeOverlay;         // Assign the FadeOverlay image
    public float fadeDuration = 1f;   // Duration for fades

    public Image cutsceneImage;
    public TextMeshProUGUI subtitlesText;
    public Sprite[] images;
    public string[] subtitles;
    public AudioClip[] narrations;
    public float[] durations; // in seconds
    public AudioSource audioSource;
    public AudioSource musicSource; // optional music
    public string nextSceneName = "MainMenu"; // or Level1

    private Coroutine cutsceneRoutine;
    private bool isSkipping = false;

    void Start()
    {
        cutsceneRoutine = StartCoroutine(PlayCutscene());
    }

    void Update()
    {
        if (!isSkipping && Input.GetKeyDown(KeyCode.Return))
        {
            isSkipping = true;

            if (cutsceneRoutine != null)
                StopCoroutine(cutsceneRoutine);

            StartCoroutine(SkipToEnd());
        }
    }
    IEnumerator Fade(bool fadeToBlack)
    {
        float startAlpha = fadeToBlack ? 0f : 1f;
        float endAlpha = fadeToBlack ? 1f : 0f;

        float t = 0f;
        Color color = fadeOverlay.color;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float blend = Mathf.Clamp01(t / fadeDuration);
            color.a = Mathf.Lerp(startAlpha, endAlpha, blend);
            fadeOverlay.color = color;
            yield return null;
        }

        color.a = endAlpha;
        fadeOverlay.color = color;
    }

    IEnumerator FadeImage(Image img, float targetAlpha, float duration)
    {
        img.gameObject.SetActive(true);
        Color original = img.color;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(original.a, targetAlpha, t / duration);
            img.color = new Color(original.r, original.g, original.b, a);
            yield return null;
        }

        img.color = new Color(original.r, original.g, original.b, targetAlpha);
    }

    IEnumerator FadeText(TextMeshProUGUI text, float targetAlpha, float duration)
    {
        text.gameObject.SetActive(true);
        Color original = text.color;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(original.a, targetAlpha, t / duration);
            text.color = new Color(original.r, original.g, original.b, a);
            yield return null;
        }

        text.color = new Color(original.r, original.g, original.b, targetAlpha);
    }

    IEnumerator PlayCutscene()
    {
        yield return StartCoroutine(Fade(false)); // fade in from black at start

        for (int i = 0; i < images.Length; i++)
        {
            // Fade to black before image change
            yield return StartCoroutine(Fade(true));

            cutsceneImage.sprite = images[i];
            subtitlesText.text = subtitles[i];
            if (narrations[i] != null)
                audioSource.PlayOneShot(narrations[i]);

            // Fade from black to image
            yield return StartCoroutine(Fade(false));

            yield return new WaitForSeconds(durations[i]);
        }

        // Final fade out from last image
        yield return StartCoroutine(Fade(true));

        // 🔇 Hide cutscene visuals so they don't flash again
        cutsceneImage.enabled = false;
        subtitlesText.enabled = false;

        // 🎬 Now show studio background
        studioBackgroundImage.gameObject.SetActive(true);


        // 🎬 Studio logo + text
        studioBackgroundImage.gameObject.SetActive(true);
        yield return StartCoroutine(FadeImage(studioBackgroundImage, 1f, textFadeDuration));
        yield return StartCoroutine(FadeText(studioText, 1f, textFadeDuration));
        yield return new WaitForSeconds(2f);

        // ⚡ Glitch flash
        yield return StartCoroutine(FlashScreen());

        // 🎮 Game title logo + text
        studioBackgroundImage.gameObject.SetActive(false);
        studioText.gameObject.SetActive(false);
        gameTitleBackgroundImage.gameObject.SetActive(true);
        yield return StartCoroutine(FadeImage(gameTitleBackgroundImage, 1f, textFadeDuration));
        yield return StartCoroutine(FadeText(gameTitleText, 1f, textFadeDuration));
        yield return new WaitForSeconds(2f);

        // Final scene transition
        LoadNextScene();
    }

    IEnumerator FlashScreen()
    {
        fadeOverlay.color = new Color(1, 1, 1, 0); // white
        fadeOverlay.enabled = true;

        float t = 0f;
        while (t < glitchFlashDuration)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(0f, 1f, t / glitchFlashDuration);
            fadeOverlay.color = new Color(1, 1, 1, a);
            yield return null;
        }

        t = 0f;
        while (t < glitchFlashDuration)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(1f, 0f, t / glitchFlashDuration);
            fadeOverlay.color = new Color(1, 1, 1, a);
            yield return null;
        }

        fadeOverlay.enabled = false;
    }

    IEnumerator SkipToEnd()
    {
        // Stop audio if needed
        if (musicSource != null) musicSource.Stop();
        if (audioSource != null) audioSource.Stop();

        cutsceneImage.enabled = false;
        subtitlesText.enabled = false;

        yield return new WaitForSeconds(0.5f);
        LoadNextScene();
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
