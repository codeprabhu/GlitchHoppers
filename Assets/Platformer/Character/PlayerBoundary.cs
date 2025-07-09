using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerBoundaryLimiter : MonoBehaviour
{
    public Transform cameraTransform;
    public float backLimit = 5f;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void LateUpdate()
    {
        if (cameraTransform == null) return;

        float minX = cameraTransform.position.x - backLimit;

        // Clamp only X axis; preserve Y movement and velocity
        if (transform.position.x < minX)
        {
            transform.position = new Vector3(minX, transform.position.y, transform.position.z);
            
            // Optional: stop leftward velocity if player hits boundary
            if (rb.linearVelocity.x < 0)
            {
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            }
        }
    }
}
