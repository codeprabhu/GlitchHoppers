using UnityEngine;
using System.Collections;

public class FireboxTrap : MonoBehaviour
{
    [Header("Settings")]
    public float triggerHoldTime = 1.5f;       // How long player must stand on it
    public float fireDuration = 2f;            // How long flames stay active
    public float cooldownTime = 3f;            // Time before it can activate again

    [Header("Flame Spawn Settings")]
    public Transform[] flameSpawnPoints;       // Where flames will be spawned
    public GameObject flamePrefab;

    [Header("Animation")]
    public Animator anim;

    private bool isTriggered = false;
    private float holdTimer = 0f;
    private bool playerOnBox = false;

    private void Update()
    {
        if (isTriggered) return;

        if (playerOnBox)
        {
            holdTimer += Time.deltaTime;
            if (holdTimer >= triggerHoldTime)
            {
                StartCoroutine(ActivateTrap());
            }
        }
        else
        {
            holdTimer = 0f;
        }
    }

    private IEnumerator ActivateTrap()
    {
        isTriggered = true;
        anim.SetTrigger("Fire"); // Trigger the activation animation

        // Spawn flames
        GameObject[] spawnedFlames = new GameObject[flameSpawnPoints.Length];
        for (int i = 0; i < flameSpawnPoints.Length; i++)
        {
            spawnedFlames[i] = Instantiate(flamePrefab, flameSpawnPoints[i].position, Quaternion.identity);
        }

        yield return new WaitForSeconds(fireDuration);

        // Destroy flames
        foreach (var flame in spawnedFlames)
        {
            if (flame != null)
                Destroy(flame);
        }

        yield return new WaitForSeconds(cooldownTime);
        isTriggered = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
            playerOnBox = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
            playerOnBox = false;
    }

    // 🔥 Gizmos to visualize flame spawn points
    private void OnDrawGizmosSelected()
    {
        if (flameSpawnPoints == null)
            return;

        Gizmos.color = Color.red;

        foreach (var point in flameSpawnPoints)
        {
            if (point != null)
            {
                Gizmos.DrawWireSphere(point.position, 0.3f);
            }
        }
    }
}
