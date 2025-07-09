using UnityEngine;
using UnityEngine.UI;

public class CooldownBar : MonoBehaviour
{
    public Image fillImage;
    public float cooldownDuration = 3f;
    public Transform player; // Assign in Inspector
    public Vector3 offset = new Vector3(0, 2.2f, 0); // Adjust as needed

    private float currentCooldown = 0f;
    private bool isVisible = false;

    void Start()
    {
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (isVisible)
        {
            currentCooldown -= Time.deltaTime;
            float fillAmount = Mathf.Clamp01(currentCooldown / cooldownDuration);
            fillImage.fillAmount = fillAmount;

            if (currentCooldown <= 0f)
            {
                HideBar();
            }
        }
    }

    void LateUpdate()
    {
        if (player != null && isVisible)
        {
            // Position the bar above the player's head
            transform.position = player.position + offset;

            // Flip the bar to match player direction
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Sign(player.localScale.x) * Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
    }


    public void StartCooldown()
    {
        currentCooldown = cooldownDuration;
        fillImage.fillAmount = 1f;
        ShowBar();
    }

    private void ShowBar()
    {
        gameObject.SetActive(true);
        isVisible = true;
    }

    private void HideBar()
    {
        gameObject.SetActive(false);
        isVisible = false;
    }
}
