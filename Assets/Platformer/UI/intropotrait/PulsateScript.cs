using UnityEngine;
using TMPro;

public class PulsateScript : MonoBehaviour
{
    public float pulseSpeed = 2f;     // How fast it pulses
    public float pulseAmount = 0.1f;  // Max scale difference

    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        float scale = 1 + Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;
        transform.localScale = originalScale * scale;
    }
}
