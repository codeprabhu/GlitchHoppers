using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFlashController : MonoBehaviour
{
    public Image flashImage;
    public float flashDuration = 0.3f;
    public Color flashColor = Color.white;

    public void Flash()
    {
        StartCoroutine(FlashRoutine());
    }

    IEnumerator FlashRoutine()
    {
        flashImage.color = new Color(flashColor.r, flashColor.g, flashColor.b, 1);
        float elapsed = 0f;

        while (elapsed < flashDuration)
        {
            float alpha = Mathf.Lerp(1, 0, elapsed / flashDuration);
            flashImage.color = new Color(flashColor.r, flashColor.g, flashColor.b, alpha);
            elapsed += Time.deltaTime;
            yield return null;
        }

        flashImage.color = new Color(flashColor.r, flashColor.g, flashColor.b, 0);
    }
}
