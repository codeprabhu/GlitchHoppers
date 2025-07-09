using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    private int currentHealth;
    private bool isDead = false;

    [Header("Heart UI")]
    public Transform heartBarParent;
    public GameObject heartPrefab;
    public Sprite fullHeart, halfHeart, emptyHeart;

    private List<Image> heartImages = new List<Image>();

    private void Start()
    {
        currentHealth = maxHealth;
        InitHearts();
        UpdateHearts();
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Max(0, currentHealth);

        // Trigger hurt animation
        if (TryGetComponent<Animator>(out Animator anim))
        {
            anim.SetTrigger("Hurt");
        }

        UpdateHearts();

        if (currentHealth <= 0)
        {
            Die();
        }
    }


    private void Die()
    {
        isDead = true;
        if (TryGetComponent<BadgerAI>(out var badger))
            badger.Die();
        else if (TryGetComponent<LeafArcherAI>(out var archer))
            archer.Die();
        else if (TryGetComponent<WizardAI>(out var wizard))
            wizard.Die();
    
}

    private void InitHearts()
    {
        int numHearts = Mathf.CeilToInt(maxHealth / 40f);
        for (int i = 0; i < numHearts; i++)
        {
            GameObject heart = Instantiate(heartPrefab, heartBarParent);
            Image heartImage = heart.GetComponent<Image>();
            heartImages.Add(heartImage);
        }
    }

    private void UpdateHearts()
    {
        int remainingHealth = currentHealth;

        for (int i = 0; i < heartImages.Count; i++)
        {
            if (remainingHealth >= 40)
            {
                heartImages[i].sprite = fullHeart;
                remainingHealth -= 40;
            }
            else if (remainingHealth >= 20)
            {
                heartImages[i].sprite = halfHeart;
                remainingHealth -= 20;
            }
            else
            {
                heartImages[i].sprite = emptyHeart;
            }
        }
    }
}
