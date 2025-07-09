using UnityEngine;

public class Bomb : MonoBehaviour
{
    [Header("World Settings")]
    [SerializeField] private bool belongsToWorldA = true;

    [Header("Explosion Settings")]
    [SerializeField] private Animator animator;
    [SerializeField] private float delayBeforeDestroy = 0.5f;
    [SerializeField] private int damageAmount = 100;

    [Header("Knockback Settings")]
    [SerializeField] private float knockbackForce = 8f; // Adjust to feel like 2-block push

    private bool exploded = false;
    private Vector3 originalScale;

    private void Start()
    {
        originalScale = transform.localScale;
        UpdateVisibility();
    }

    private void Update()
    {
        UpdateVisibility();
    }

    private void UpdateVisibility()
    {
        if (NewTileSwitcher.Instance == null)
            return;

        bool shouldBeActive = NewTileSwitcher.Instance.isInWorldA == belongsToWorldA;

        // Control visibility and collider
        GetComponent<SpriteRenderer>().enabled = shouldBeActive;
        GetComponent<Collider2D>().enabled = shouldBeActive;
        transform.localScale = shouldBeActive ? originalScale : Vector3.zero;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!exploded && other.CompareTag("Player"))
        {
            exploded = true;
            animator.SetTrigger("Explode");

            // Optional: Deal damage
            IDamageable damageable = other.GetComponent<IDamageable>();
            if (damageable != null)
                damageable.TakeDamage(damageAmount);

            // Apply knockback
            Rigidbody2D playerRb = other.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                Vector2 direction = (other.transform.position - transform.position).normalized;
                direction = new Vector2(direction.x, 0f).normalized; // Horizontal knockback only
                Vector2 knockbackVelocity = direction * knockbackForce;
                knockbackVelocity.y = 20f; // or whatever you want for a small hop back
                playerRb.linearVelocity = knockbackVelocity;

            }

            Destroy(gameObject, delayBeforeDestroy);
        }
    }
}
