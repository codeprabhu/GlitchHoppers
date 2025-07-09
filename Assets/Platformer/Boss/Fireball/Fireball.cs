using UnityEngine;

public class FireballScript : MonoBehaviour
{
    [Header("Targeting")]
    public Transform player;
    public float homingSpeed = 6f;
    public float lockedSpeed = 10f;
    public float lockOnRadius = 2f;

    [Header("Damage")]
    public int damage = 10;
    public LayerMask hitLayers; // Set this to Player + Ground layers

    [Header("Lifetime")]
    public float maxLifeTime = 8f;
    private float lifeTimer = 0f;

    private Rigidbody2D rb;
    private bool directionLocked = false;
    private Vector2 lockedDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (!player) return;

        lifeTimer += Time.deltaTime;
        if (lifeTimer > maxLifeTime)
        {
            Destroy(gameObject);
            return;
        }

        if (!directionLocked)
        {
            float distance = Vector2.Distance(transform.position, player.position);

            if (distance <= lockOnRadius)
            {
                // Lock direction permanently
                lockedDirection = (player.position - transform.position).normalized;
                directionLocked = true;
            }
            else
            {
                // Keep homing toward player
                Vector2 dir = (player.position - transform.position).normalized;
                rb.linearVelocity = dir * homingSpeed;
                RotateTowards(dir);
                return;
            }
        }

        // Move in locked direction
        rb.linearVelocity = lockedDirection * lockedSpeed;
        RotateTowards(lockedDirection);
    }

    void RotateTowards(Vector2 dir)
    {
        if (dir.sqrMagnitude == 0) return;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & hitLayers) == 0)
            return;

        if (other.CompareTag("Player"))
        {
            IDamageable dmg = other.GetComponent<IDamageable>();
            if (dmg != null)
                dmg.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
