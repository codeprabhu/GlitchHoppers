using UnityEngine;

public class Arrow : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int damage = 10;
    [SerializeField] private float lifetime = 5f;
    [SerializeField] private LayerMask hitLayers;

    [Header("Optional FX")]
    [SerializeField] private GameObject hitEffect;
    [SerializeField] private AudioClip hitSound;

    private BoxCollider2D boxCollider;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    public void SetDirection(Vector2 dir)
    {
        if (rb != null)
            rb.linearVelocity = dir * 10f;

        // Flip sprite and collider
        if (dir.x < 0)
        {
            // Flip sprite
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * -1f;
            transform.localScale = scale;

            // Flip collider offset
            if (boxCollider != null)
            {
                Vector2 offset = boxCollider.offset;
                offset.x = -Mathf.Abs(offset.x);
                boxCollider.offset = offset;
            }
        }
        else
        {
            // Reset to normal
            if (boxCollider != null)
            {
                Vector2 offset = boxCollider.offset;
                offset.x = Mathf.Abs(offset.x);
                boxCollider.offset = offset;
            }
        }
    }    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if we hit a valid layer
        if (((1 << collision.gameObject.layer) & hitLayers) != 0)
        {
            // Deal damage if target is damageable
            IDamageable target = collision.GetComponent<IDamageable>();
            if (target != null)
                target.TakeDamage(damage);

            // Spawn hit effect
            if (hitEffect != null)
                Instantiate(hitEffect, transform.position, Quaternion.identity);

            // Play sound effect
            if (hitSound != null)
                AudioSource.PlayClipAtPoint(hitSound, transform.position);

            // Destroy arrow
            Destroy(gameObject);
        }
    }
}
