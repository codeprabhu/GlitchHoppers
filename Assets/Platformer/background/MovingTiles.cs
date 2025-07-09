using UnityEngine;

public class MovingTiles : MonoBehaviour
{
    [Header("Path")]
    [SerializeField] private Vector2[] localPoints;
    [SerializeField] private float speed = 2f;

    private Vector2 startPosition;
    private int currentIndex = 0;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        // Move only if platform is active in hierarchy
        if (!gameObject.activeInHierarchy)
            return;

        Vector2 target = startPosition + localPoints[currentIndex];
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, target) < 0.05f)
        {
            currentIndex = (currentIndex + 1) % localPoints.Length;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (localPoints == null || localPoints.Length == 0)
            return;

        Gizmos.color = Color.cyan;
        Vector2 origin = Application.isPlaying ? startPosition : (Vector2)transform.position;
        Vector2 previous = origin;

        for (int i = 0; i < localPoints.Length; i++)
        {
            Vector2 current = origin + localPoints[i];
            Gizmos.DrawWireSphere(current, 0.1f);
            Gizmos.DrawLine(previous, current);
            previous = current;
        }

        // Optionally close loop
        Gizmos.DrawLine(previous, origin + localPoints[0]);
    }
}
