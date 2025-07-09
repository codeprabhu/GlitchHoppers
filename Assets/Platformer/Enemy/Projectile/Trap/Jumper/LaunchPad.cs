using UnityEngine;

public class LaunchPad : MonoBehaviour
{
    public Vector2 launchVelocity = new Vector2(0, 10);
    public LayerMask playerLayer;
    public Animator animator;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject other = collision.gameObject;

        if (((1 << other.layer) & playerLayer) != 0)
        {
            animator?.SetTrigger("Jump");
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = launchVelocity;
                Debug.Log($"[LaunchPad] Launched {other.name} with velocity: {launchVelocity}");
            }

            
        }
    }
}
