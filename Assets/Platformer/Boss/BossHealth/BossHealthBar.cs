using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BossHealthBar : MonoBehaviour
{
    public Image fillImage;              // Assign HealthFill here
    public RectTransform barTransform;  // Assign BossHealthBar (the parent image's RectTransform)

    public Color phase1Color = Color.green;
    public Color phase2Color = Color.yellow;
    public Color phase3Color = Color.red;

    public float shakeDuration = 0.2f;
    public float shakeMagnitude = 10f;

    private Vector3 originalPos;

    void Start()
    {
        if (barTransform != null)
            originalPos = barTransform.anchoredPosition;
    }

    public void SetHealth(int current, int max)
    {
        float pct = (float)current / max;
        fillImage.fillAmount = pct;

        if (pct > 0.66f)
            fillImage.color = phase1Color;
        else if (pct > 0.33f)
            fillImage.color = phase2Color;
        else
            fillImage.color = phase3Color;

        if (barTransform != null)
            StartCoroutine(ShakeBar());
    }

    private IEnumerator ShakeBar()
    {
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float offsetX = Random.Range(-1f, 1f) * shakeMagnitude;
            float offsetY = Random.Range(-1f, 1f) * shakeMagnitude;

            barTransform.anchoredPosition = originalPos + new Vector3(offsetX, offsetY, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        barTransform.anchoredPosition = originalPos;
    }
}
