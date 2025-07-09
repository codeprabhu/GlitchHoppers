using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public int maxHealth = 100;
    private int currentHealth;

    [SerializeField] private Image healthFillImage;

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();
        Debug.Log("Player took damage!");

        if (currentHealth <= 0)
        {
            Die();
        }
    }


    private void UpdateHealthBar()
    {
        if (healthFillImage != null)
        {
            float fillAmount = (float)currentHealth / maxHealth;
            Debug.Log($"Updating Health Bar: {currentHealth}/{maxHealth} = {fillAmount}");
            healthFillImage.fillAmount = fillAmount;
        }
    }


    [SerializeField] private GameObject deathCanvas; // assign in Inspector

    private void Die()
    {
        Debug.Log("Player died");

        if (deathCanvas != null)
        {
            deathCanvas.SetActive(true);
        }

        Time.timeScale = 0f;
    }

    public void ApplyKnockback(Vector2 force)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero; // optional: reset current motion
            rb.AddForce(force, ForceMode2D.Impulse);
        }
    }

}
