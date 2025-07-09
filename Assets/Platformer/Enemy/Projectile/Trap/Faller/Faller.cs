using UnityEngine;

public class FallingPlatformReset : MonoBehaviour
{
    [SerializeField] private float yResetThreshold = -100f;

    private Vector3 originalPosition;
    private Rigidbody2D rb;
    private bool falling = false;

    void Awake()
    {
        originalPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
    }

    void Update()
    {
        if (falling && transform.position.y < yResetThreshold)
        {
            ResetPlatform();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!falling && collision.collider.CompareTag("Player"))
        {
            falling = true;
            rb.gravityScale = 1f;
        }
    }

    void ResetPlatform()
    {
        rb.gravityScale = 0f;
        rb.linearVelocity = Vector2.zero;
        transform.position = originalPosition;
        falling = false;
    }
}
