using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NewTileSwitcher : MonoBehaviour
{
    public static NewTileSwitcher Instance;

    [Header("Main World Tilemaps")]
    public GameObject tilemapA;
    public GameObject tilemapB;

    [Header("Spikes for Each World")]
    public GameObject spikesA;
    public GameObject spikesB;

    [Header("Moving Platforms")]
    public GameObject movingPlatformsA;
    public GameObject movingPlatformsB;

    [Header("Flash & Camera")]
    public CanvasGroup flashPanel; // Assign in Inspector
    public Image flashImage;       // Assign the FlashPanel's Image
    public CameraShake cameraShake;

    public bool isInWorldA = true;

    void Awake()
    {
        Instance = this;
        flashPanel.alpha = 0f;
        SetTilemapState(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isInWorldA = !isInWorldA;
            SetTilemapState(isInWorldA);

            Color flashColor = isInWorldA ? Color.white : Color.black;
            StartCoroutine(FlashScreen(flashColor));
            StartCoroutine(cameraShake.Shake(0.2f, 0.15f));
        }
    }

    void SetTilemapState(bool isWorldA)
    {
        // Ground
        if (tilemapA) tilemapA.SetActive(isWorldA);
        if (tilemapB) tilemapB.SetActive(!isWorldA);

        // Spikes
        if (spikesA) spikesA.SetActive(isWorldA);
        if (spikesB) spikesB.SetActive(!isWorldA);

        // Moving Platforms
        if (movingPlatformsA) movingPlatformsA.SetActive(isWorldA);
        if (movingPlatformsB) movingPlatformsB.SetActive(!isWorldA);
    }

    IEnumerator FlashScreen(Color color)
    {
        flashImage.color = color;

        float duration = 0.1f;
        float t = 0f;

        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            flashPanel.alpha = Mathf.Lerp(0f, 1f, t / duration);
            yield return null;
        }

        yield return new WaitForSecondsRealtime(0.05f);

        t = 0f;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            flashPanel.alpha = Mathf.Lerp(1f, 0f, t / duration);
            yield return null;
        }

        flashPanel.alpha = 0f;
    }
}
