using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TilemapSwitcher : MonoBehaviour
{
    public GameObject tilemapA;
    public GameObject tilemapB;

    public CanvasGroup flashPanel; // Assign in Inspector
    public Image flashImage;       // Assign the FlashPanel's Image

    private bool isInWorldA = true;
    public CameraShake cameraShake;

    void Start()
    {
        SetTilemapState(true);
        flashPanel.alpha = 0f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            bool switchingToA = isInWorldA == false; // if we’re going back to A, use white
            isInWorldA = !isInWorldA;
            SetTilemapState(isInWorldA);

            Color flashColor = switchingToA ? Color.white : Color.black;
            StartCoroutine(FlashScreen(flashColor));
            StartCoroutine(cameraShake.Shake(0.2f, 0.15f)); 
        }
    }

    void SetTilemapState(bool isWorldA)
    {
        tilemapA.SetActive(isWorldA);
        tilemapB.SetActive(!isWorldA);
    }

    IEnumerator FlashScreen(Color color)
    {
        flashImage.color = color;

        float duration = 0.1f;
        float t = 0f;

        // Fade in
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            flashPanel.alpha = Mathf.Lerp(0f, 1f, t / duration);
            yield return null;
        }

        yield return new WaitForSecondsRealtime(0.05f);

        // Fade out
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
