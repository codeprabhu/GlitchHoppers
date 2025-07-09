using UnityEngine;

public class BadgerAttackTrigger : MonoBehaviour
{
    private BadgerAI badgerAI;

    [SerializeField] private float attackRadius = 0.6f; // Set this to match your attack area

    private void Awake()
    {
        badgerAI = GetComponentInParent<BadgerAI>();
    }

    /// <summary>
    /// Called from the "pound" moment in the Badger's attack animation.
    /// Deals damage to the player if they are in range.
    /// </summary>
    public void EnableDamage()
    {
        if (badgerAI == null) return;

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRadius, badgerAI.playerLayer);

        foreach (Collider2D col in hits)
        {   
            if (col.CompareTag("Player"))
            {
                PlayerHealth player = col.GetComponent<PlayerHealth>();
                if (player != null)
                {
                    player.TakeDamage(badgerAI.damage);

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
