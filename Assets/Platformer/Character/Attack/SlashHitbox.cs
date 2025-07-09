using UnityEngine;
using System.Collections;

public class SlashHitbox : MonoBehaviour
{
    [SerializeField] public LayerMask enemyLayer;
    private int damage;

    public void ActivateHitbox(int newDamage)
    {
        damage = newDamage;

        gameObject.SetActive(true); // 🟢 Activate first

        StartCoroutine(DelayedOverlap()); // ⏳ Wait one frame
    }

    private IEnumerator DelayedOverlap()
    {
        yield return null; // ✅ Let Unity refresh physics

        Collider2D[] results = new Collider2D[10];
        ContactFilter2D filter = new ContactFilter2D
        {
            useTriggers = true,
            useLayerMask = true,
            layerMask = enemyLayer
        };

        int hitCount = Physics2D.OverlapCollider(GetComponent<Collider2D>(), filter, results);
        Debug.Log("Enemies hit: " + hitCount);

        for (int i = 0; i < hitCount; i++)
        {
            var target = results[i].GetComponent<IDamageable>();
            if (target != null)
            {
                Debug.Log("Damaging: " + results[i].name);
                target.TakeDamage(damage);
                break; // ✅ Only one enemy
            }
        }

        Invoke(nameof(DeactivateHitbox), 0.1f);
    }

    private void DeactivateHitbox()
    {
        gameObject.SetActive(false);
    }
}
