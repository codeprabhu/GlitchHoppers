using UnityEngine;

public class WizardAI : MonoBehaviour, IDamageable
{
    [Header("Detection & Layers")]
    [SerializeField] public LayerMask playerLayer;

    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform playerSpawnPoint; // NEW
    [SerializeField] private WizardAttackTrigger attackTrigger;

    [Header("Stats")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float patrolRange = 6f;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] public int damage = 5;

    [Header("Teleport Settings")]
    [SerializeField] private float wizardTeleportRadius = 4f;
    [SerializeField] private float playerTeleportCheckRadius = 0.3f;
    [SerializeField] private LayerMask teleportValidLayers;
    [SerializeField] private int maxHitsBeforePlayerTeleport = 3;
    [Header("Teleport Points")]
    [SerializeField] private Transform[] teleportPoints;
    private Transform lastTeleportPoint;


    private int successfulHits = 0;
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer spriteRenderer;
    private bool isDead = false;
    private bool isAttacking = false;
    private float lastAttackTime = -999f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (isDead || player == null) return;

        if (isAttacking)
        {
            AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
            if (!state.IsName("Attack") || state.normalizedTime >= 1f)
                isAttacking = false;
            else
                return;
        }

        float distance = Mathf.Abs(player.position.x - transform.position.x);

        if (distance > patrolRange)
        {
            StopMovement();
            anim.SetBool("Idle", true);
        }
        else if (distance > attackRange)
        {
            MoveTowardPlayer();
        }
        else
        {
            StopMovement();
            if (Time.time - lastAttackTime >= attackCooldown)
                Attack();
        }

        float velocityX = Mathf.Abs(rb.linearVelocity.x);
        bool isMoving = velocityX > 0.1f;

        anim.SetBool("Run", isMoving);
        anim.SetBool("Idle", !isMoving);

    }

    private void MoveTowardPlayer()
    {
        anim.SetBool("Run", true);
        float direction = Mathf.Sign(player.position.x - transform.position.x);
        rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);
        FlipSprite(direction);
    }

    private void StopMovement()
    {
        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        anim.SetBool("Idle", true);
    }

    private void Attack()
    {
        anim.SetTrigger("Attack");
        isAttacking = true;
        lastAttackTime = Time.time;

        // Deal damage
        player.GetComponent<IDamageable>()?.TakeDamage(damage);

        successfulHits++;

        // Wizard teleports after every 2 hits
        if (successfulHits % 2 == 0)
        {
            TeleportWizardToRandomPoint();
        }

        // After 3 hits, teleport the player
        if (successfulHits >= maxHitsBeforePlayerTeleport)
        {
            TeleportPlayerToSpawn();
            successfulHits = 0;
        }
    }


    private void TeleportWizardToRandomPoint()
    {
        if (teleportPoints.Length == 0)
        {
            Debug.LogWarning("No teleport points assigned.");
            return;
        }

        // Choose a new point different from last teleport location
        Transform chosenPoint = null;
        int attempts = 10;

        while (attempts-- > 0)
        {
            Transform randomPoint = teleportPoints[Random.Range(0, teleportPoints.Length)];
            if (randomPoint != lastTeleportPoint)
            {
                chosenPoint = randomPoint;
                break;
            }
        }

        if (chosenPoint == null)
            chosenPoint = teleportPoints[0]; // fallback

        transform.position = chosenPoint.position;
        anim.SetTrigger("Teleport"); // Optional animation
        lastTeleportPoint = chosenPoint;
    }


    private void TeleportPlayerToSpawn()
    {
        if (player != null && playerSpawnPoint != null)
        {
            player.position = playerSpawnPoint.position;
            // Optional effects: screen flash, sfx, etc.
        }
    }

    private void FlipSprite(float direction)
    {
        transform.localScale = new Vector3(direction > 0.01f ? 1 : -1, 1, 1);
    }

    public void TakeHit(int damage)
    {
        GetComponent<EnemyHealth>()?.TakeDamage(damage);
        anim.SetTrigger("Hurt");
    }

    public void Die()
    {
        isDead = true;
        rb.linearVelocity = Vector2.zero;
        anim.SetTrigger("Die");
        Destroy(gameObject, 1.5f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, patrolRange);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, wizardTeleportRadius);
    }

    public void EnablePoundHitbox() => attackTrigger?.EnableDamage();
    public void DisablePoundHitbox() => attackTrigger?.DisableDamage();
    public void TakeDamage(int damage) => TakeHit(damage);
}
