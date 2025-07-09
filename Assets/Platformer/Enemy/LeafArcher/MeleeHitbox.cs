using UnityEngine;

public class MeleeHitbox : MonoBehaviour
{
    private int damage;
    private bool isActive = false;
    private Collider2D hitboxCollider;

    private void Awake()
    {
        hitboxCollider = GetComponent<Collider2D>();
        hitboxCollider.enabled = false; // Ensure disabled on start
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isActive) return;

        if (collision.CompareTag("Player")) // Or enemy if used for player attack
        {
            IDamageable target = collision.GetComponent<IDamageable>();
            if (target != null)
            {
                target.TakeDamage(damage);
                isActive = false;
                hitboxCollider.enabled = false; // Ensure no further hits
            }
        }
    }

    public void ActivateHitbox(int dmg)
    {
        damage = dmg;
        isActive = true;
        hitboxCollider.enabled = true;
        return;
    }

    public void DisableHitbox()
    {
        isActive = false;
        hitboxCollider.enabled = false;
        return;
    }
}
