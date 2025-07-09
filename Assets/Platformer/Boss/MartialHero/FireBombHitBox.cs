using UnityEngine;

public class FirebombDamage : MonoBehaviour
{
    public int damage = 30;
    public float knockbackForce = 10f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            IDamageable dmg = other.GetComponent<IDamageable>();
            if (dmg != null)
                dmg.TakeDamage(damage);

            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 knockDir = (other.transform.position - transform.position).normalized;
                rb.linearVelocity = Vector2.zero; // Reset before knockback
                rb.AddForce(knockDir * knockbackForce, ForceMode2D.Impulse);
            }
        }
    }
}
