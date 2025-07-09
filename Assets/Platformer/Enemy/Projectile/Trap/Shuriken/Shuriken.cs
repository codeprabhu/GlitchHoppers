using System.Collections.Generic;
using UnityEngine;

public class Shuriken : MonoBehaviour
{
    [Header("Path Settings")]
    public List<Vector2> pathPoints; // Relative positions from the start
    public float speed = 2f;

    [Header("Damage Settings")]
    public int damage = 20;
    public string playerTag = "Player";

    private int currentIndex = 0;
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        if (pathPoints == null || pathPoints.Count == 0) return;

        Vector3 target = startPos + (Vector3)pathPoints[currentIndex];
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) < 0.05f)
        {
            currentIndex = (currentIndex + 1) % pathPoints.Count;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            IDamageable damageable = other.GetComponent<IDamageable>();
            if (damageable != null)
                damageable.TakeDamage(damage);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (pathPoints == null || pathPoints.Count == 0) return;

        Gizmos.color = Color.red;

        Vector3 origin = Application.isPlaying ? startPos : transform.position;

        for (int i = 0; i < pathPoints.Count; i++)
        {
            Vector3 point = origin + (Vector3)pathPoints[i];
            Gizmos.DrawSphere(point, 0.15f);

            Vector3 nextPoint = origin + (Vector3)pathPoints[(i + 1) % pathPoints.Count];
            Gizmos.DrawLine(point, nextPoint);
        }
    }
}
