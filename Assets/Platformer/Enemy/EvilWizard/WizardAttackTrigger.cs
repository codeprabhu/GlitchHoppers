using UnityEngine;

public class WizardAttackTrigger : MonoBehaviour
{
    private WizardAI wizardAI;

    [SerializeField] private float attackRadius = 0.6f; // Set this to match your attack area

    private void Awake()
    {
        wizardAI = GetComponentInParent<WizardAI>();
    }
    public void EnableDamage()
    {
        if (wizardAI == null) return;

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRadius, wizardAI.playerLayer);

        foreach (Collider2D col in hits)
        {   
            if (col.CompareTag("Player"))
            {
                PlayerHealth player = col.GetComponent<PlayerHealth>();
                if (player != null)
                {
                    player.TakeDamage(wizardAI.damage);

                    // Calculate knockback direction from badger to player
                    // Calculate knockback direction from badger to player (horizontal only)
                    float directionX = Mathf.Sign(col.transform.position.x - transform.position.x);
                    Vector2 knockbackDirection = new Vector2(directionX, 0f);
                    float knockbackForce = 1f; // tweak as needed

                    player.ApplyKnockback(knockbackDirection * knockbackForce);
                }
            }
        }
    }


    // Optional: called at the end of the attack animation if needed
    public void DisableDamage() { }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
