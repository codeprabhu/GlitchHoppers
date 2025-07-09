using UnityEngine;

public class WhirlwindHitbox : MonoBehaviour
{
    public float knockbackX = 20f;
    public float knockbackY = 8f;
    public int damage = 10;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                Vector2 direction = (other.transform.position - transform.position).normalized;
                Vector2 knockback = new Vector2(direction.x * knockbackX, knockbackY);

                rb.linearVelocity = knockback; // 🔥 instant launch, drag doesn't affect
                Debug.Log("Whirlwind knockback velocity: " + knockback);
            }

            IDamageable dmg = other.GetComponent<IDamageable>();
            if (dmg != null) dmg.TakeDamage(damage);
        }
    }
}
