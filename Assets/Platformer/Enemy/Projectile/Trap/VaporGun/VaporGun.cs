using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VaporGun : MonoBehaviour
{
    [Header("Laser Settings")]
    public Transform laserBeam;
    public float growSpeed = 5f;
    public float holdDuration = 2f;
    public float cooldownDuration = 2f;

    [Header("Damage Settings")]
    public int damage = 10;
    public float damageInterval = 0.5f;
    public LayerMask playerLayer;

    private BoxCollider2D laserCollider;
    private Vector3 fullScale;
    public float startDelay = 1f; // Delay before the laser starts firing

    private bool damaging = false;
    private Dictionary<Collider2D, float> damageTimers = new();

    void Start()
    {
        
        laserCollider = laserBeam.GetComponent<BoxCollider2D>();
        fullScale = laserBeam.localScale;

        laserBeam.localScale = new Vector3(0f, fullScale.y, fullScale.z);
        laserBeam.gameObject.SetActive(false);

        StartCoroutine(FireLoop());
    }

    private IEnumerator FireLoop()
    {
        yield return new WaitForSeconds(startDelay);
        while (true)
        {
            laserBeam.gameObject.SetActive(true);
            damaging = false;

            float scaleX = 0f;

            while (scaleX < Mathf.Abs(fullScale.x))
            {
                scaleX += growSpeed * Time.deltaTime;
                laserBeam.localScale = new Vector3(scaleX, fullScale.y, fullScale.z);
                yield return null;
            }

            damaging = true;
            yield return new WaitForSeconds(holdDuration);

            laserBeam.localScale = new Vector3(0f, fullScale.y, fullScale.z);
            laserBeam.gameObject.SetActive(false);
            damaging = false;

            damageTimers.Clear(); // Reset damage timers between pulses
            yield return new WaitForSeconds(cooldownDuration);
        }
    }

    public void HandleTriggerStay(Collider2D other)
    {
        if (!damaging) return;

        if ((playerLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            IDamageable dmg = other.GetComponent<IDamageable>();
            if (dmg == null) return;

            float lastTime;
            damageTimers.TryGetValue(other, out lastTime);

            if (Time.time - lastTime >= damageInterval)
            {
                dmg.TakeDamage(damage);
                damageTimers[other] = Time.time;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Clean up when player leaves the laser
        if (damageTimers.ContainsKey(other))
        {
            damageTimers.Remove(other);
        }
    }
}
