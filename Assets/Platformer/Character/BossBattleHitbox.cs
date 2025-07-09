using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class BossHitboxPlayer : MonoBehaviour
{
    [Header("Settings")]
    public LayerMask enemyLayer;
    private int damage = 100;
    private bool active = false;

    private Collider2D hitboxCollider;

    private void Awake()
    {
        hitboxCollider = GetComponent<Collider2D>();
        hitboxCollider.enabled = false;
    }

    public void ActivateHitbox(int damageAmount)
    {
        damage = damageAmount;
        active = true;
        hitboxCollider.enabled = true;

        StartCoroutine(DelayedOverlap());
    }

    private IEnumerator DelayedOverlap()
    {
        yield return null; // Wait one frame for physics update

        Collider2D[] results = new Collider2D[10];
        ContactFilter2D filter = new ContactFilter2D
        {
            useTriggers = true,
            useLayerMask = true,
            layerMask = enemyLayer
        };

        int hitCount = Physics2D.OverlapCollider(hitboxCollider, filter, results);
        Debug.Log("Slash hit: " + hitCount + " enemies");

        for (int i = 0; i < hitCount; i++)
        {
            Collider2D hit = results[i];

            if (hit.CompareTag("BossHead"))
            {
                BossSamuraiController boss = hit.GetComponentInParent<BossSamuraiController>();
                boss.TakeHeadDamage(damage);
                break;
            }
            else if (hit.CompareTag("BossBody"))
            {
                BossSamuraiController boss = hit.GetComponentInParent<BossSamuraiController>();
                boss.TakeBodyHit();
                break;
            }
        }

        Invoke(nameof(DeactivateHitbox), 0.1f);
    }

    private void DeactivateHitbox()
    {
        active = false;
        hitboxCollider.enabled = false;
    }
}
