using Unity.VisualScripting;
using UnityEngine;

public class BossHealth : MonoBehaviour, IDamageable
{
    public int maxHealth = 300;
    private int currentHealth;

    public BossSamuraiController bossController; // Reference to the boss controller
    public BossHealthBar bossHealthBar;
    public BossPhaseManager phaseManager;

    void Start()
    {
        currentHealth = maxHealth;
        bossHealthBar.SetHealth(currentHealth, maxHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(0, currentHealth);

        bossHealthBar.SetHealth(currentHealth, maxHealth);
        phaseManager.CheckPhase(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Boss is dead!");
        GetComponent<BossSamuraiController>().Die();
        // Do death animation, disable scripts, etc.
    }
}
